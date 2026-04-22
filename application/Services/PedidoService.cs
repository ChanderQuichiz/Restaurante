using application.Data;
using application.Dtos;
using application.Enums;
using application.Models;
using application.Utils;
using Microsoft.EntityFrameworkCore;

namespace application.Services;

public class PedidoService : IPedidoService
{
    private readonly DbAppContext context;
    private const int PageSize = 10;

    public PedidoService(DbAppContext context)
    {
        this.context = context;
    }

    public async Task<PedidoVM> obtenerPedidosVM(int page = 1, string? buscar = null, EstadoPedidoEnum? estado = null, DateTime? fecha = null)
    {
        page = page < 1 ? 1 : page;

        IQueryable<PedidoModel> pedidos = context.Pedidos
            .Include(p => p.Mesero)
            .Include(p => p.Mesa)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            string correlativoBuscado = buscar.Trim();
            pedidos = pedidos.Where(p => p.correlativo.Contains(correlativoBuscado) || (p.dniCliente != null && p.dniCliente.Contains(correlativoBuscado)));
        }

        if (estado.HasValue)
        {
            pedidos = pedidos.Where(p => p.estado == mapearEstadoTexto(estado.Value));
        }

        if (fecha.HasValue)
        {
            DateTime soloFecha = fecha.Value.Date;
            pedidos = pedidos.Where(p => p.fecha.Date == soloFecha);
        }

        var totalPedidos = await pedidos.CountAsync();
        var totalPages = (int)Math.Ceiling(totalPedidos / (double)PageSize);
        totalPages = totalPages == 0 ? 1 : totalPages;

        List<PedidoModel> lista = await pedidos
            .OrderBy(p => p.id)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        return new PedidoVM(
            lista.Select(PedidoMapper.ToPedidoDto).ToList(),
            page,
            totalPages,
            buscar,
            estado,
            fecha
        );
    }

    public async Task<PedidoDto> crearPedido(CrearPedidoDto crearPedidoDto)
    {
        PedidoModel pedido = PedidoMapper.ToPedidoModel(crearPedidoDto);
        pedido.correlativo = await ObtenerSiguienteCorrelativoAsync();
        pedido.fecha = DateTime.Now;
        pedido.estado = crearPedidoDto.estado switch
        {
            EstadoPedidoEnum.Pendiente => "Pendiente",
            EstadoPedidoEnum.EnProceso => "En proceso",
            EstadoPedidoEnum.Entregado => "Entregado",
            _ => "Pendiente"
        };

        double total = 0;
        for (int i = 0; i < crearPedidoDto.platoIds.Count; i++)
        {
            total += crearPedidoDto.cantidades[i] * crearPedidoDto.precios[i];
        }

        pedido.total = total;

        await context.Pedidos.AddAsync(pedido);
        await context.SaveChangesAsync();

        for (int i = 0; i < crearPedidoDto.platoIds.Count; i++)
        {
            DetallePedidoModel detalle = new DetallePedidoModel
            {
                pedidoId = pedido.id,
                platoId = crearPedidoDto.platoIds[i],
                cantidad = crearPedidoDto.cantidades[i],
                precioUnitario = crearPedidoDto.precios[i]
            };

            await context.DetallePedidos.AddAsync(detalle);
        }

        await context.SaveChangesAsync();

        UsuarioModel? mesero = await context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.id == pedido.meseroId);

        if (mesero != null)
        {
            pedido.Mesero = mesero;
        }

        return PedidoMapper.ToPedidoDto(pedido);
    }

    public async Task<PedidoDetalleVM?> obtenerDetallePedido(int pedidoId)
    {
        PedidoModel? pedido = await context.Pedidos
            .Include(p => p.Mesero)
            .Include(p => p.Detalles)
            .ThenInclude(d => d.Plato)
            .FirstOrDefaultAsync(p => p.id == pedidoId);

        if (pedido == null)
        {
            return null;
        }

        List<DetallePedidoDto> detalles = pedido.Detalles
            .Select(PedidoMapper.ToDetallePedidoDto)
            .ToList();

        return new PedidoDetalleVM(
            PedidoMapper.ToPedidoDto(pedido),
            detalles,
            pedido.Mesero.nombre
        );
    }

    public async Task<List<UsuarioModel>> obtenerMeserosActivos()
    {
        return await context.Usuarios
            .Where(u => u.estado == "Activo" && u.rol == "Mesero")
            .OrderBy(u => u.nombre)
            .ToListAsync();
    }

    public async Task<List<MesaModel>> obtenerMesasDisponibles()
    {
        return await context.Mesas
            .Where(m => m.estado == EstadoMesaEnum.LIBRE.ToString())
            .OrderBy(m => m.correlativo)
            .ToListAsync();
    }

    public async Task<List<PlatoModel>> obtenerPlatosActivos()
    {
        return await context.Platos
            .Where(p => p.estado == "Activo")
            .OrderBy(p => p.nombre)
            .ToListAsync();
    }

    private static string mapearEstadoTexto(EstadoPedidoEnum estado)
    {
        return estado switch
        {
            EstadoPedidoEnum.Pendiente => "Pendiente",
            EstadoPedidoEnum.EnProceso => "En proceso",
            EstadoPedidoEnum.Entregado => "Entregado",
            _ => "Pendiente"
        };
    }

    private async Task<string> ObtenerSiguienteCorrelativoAsync()
    {
        var ultimoCorrelativo = await context.Pedidos
            .AsNoTracking()
            .Where(p => !string.IsNullOrWhiteSpace(p.correlativo))
            .OrderByDescending(p => p.id)
            .Select(p => p.correlativo)
            .FirstOrDefaultAsync();

        if (!string.IsNullOrWhiteSpace(ultimoCorrelativo))
        {
            return ConstruirCorrelativo(ultimoCorrelativo, "PED");
        }

        var totalPedidos = await context.Pedidos.CountAsync();
        return $"PED-{totalPedidos + 1:D2}";
    }

    private static string ConstruirCorrelativo(string correlativoActual, string prefijo)
    {
        var partes = correlativoActual.Split('-', 2);

        if (partes.Length == 2 && int.TryParse(partes[1], out var numero))
        {
            return $"{prefijo}-{numero + 1:D2}";
        }

        return $"{prefijo}-01";
    }
}