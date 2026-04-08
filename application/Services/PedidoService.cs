using application.Data;
using application.Dtos;
using application.Models;
using application.Utils;
using Microsoft.EntityFrameworkCore;

namespace application.Services;

public class PedidoService : IPedidoService
{
    private readonly DbAppContext context;

    public PedidoService(DbAppContext context)
    {
        this.context = context;
    }

    public async Task<List<PedidoDto>> obtenerPedidos(string? buscar, string? estado, DateTime? fecha)
    {
        IQueryable<PedidoModel> pedidos = context.Pedidos
            .Include(p => p.Mesero)
            .Include(p => p.Mesa)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            string dniBuscado = buscar.Trim();
            pedidos = pedidos.Where(p => p.dniCliente != null && p.dniCliente.Contains(dniBuscado));
        }

        if (!string.IsNullOrWhiteSpace(estado) && estado != "Todos")
        {
            pedidos = pedidos.Where(p => p.estado == estado);
        }

        if (fecha.HasValue)
        {
            DateTime soloFecha = fecha.Value.Date;
            pedidos = pedidos.Where(p => p.fecha.Date == soloFecha);
        }

        List<PedidoModel> lista = await pedidos
            .OrderBy(p => p.id)
            .ToListAsync();

        return lista
            .Select(PedidoMapper.ToPedidoDto)
            .ToList();
    }

    public async Task<PedidoDto> crearPedido(CrearPedidoDto crearPedidoDto)
    {
        PedidoModel pedido = PedidoMapper.ToPedidoModel(crearPedidoDto);
        pedido.fecha = DateTime.Now;
        pedido.estado = crearPedidoDto.estado;

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
            .Where(m => m.estado == "Disponible")
            .OrderBy(m => m.id)
            .ToListAsync();
    }

    public async Task<List<PlatoModel>> obtenerPlatosActivos()
    {
        return await context.Platos
            .Where(p => p.estado == "Activo")
            .OrderBy(p => p.nombre)
            .ToListAsync();
    }
}