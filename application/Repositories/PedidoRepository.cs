using application.Data;
using application.Enums;
using application.Models;
using Microsoft.EntityFrameworkCore;

namespace application.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly DbAppContext _context;

    public PedidoRepository(DbAppContext context)
    {
        _context = context;
    }


    public async Task<PedidoModel?> GetById(int id)
        => await _context.Pedidos.FirstOrDefaultAsync(p => p.id == id);

    public async Task<PedidoModel> Add(PedidoModel pedido)
    {
        await _context.Pedidos.AddAsync(pedido);
        await _context.SaveChangesAsync();          

        pedido.correlativo = $"PED-{pedido.id:D3}";
        await _context.SaveChangesAsync();          

        return pedido;
    }

    public async Task Update()
        => await _context.SaveChangesAsync();

    public async Task Delete(PedidoModel pedido)
    {
        _context.Pedidos.Remove(pedido);
        await _context.SaveChangesAsync();
    }


    public async Task<List<PedidoModel>> GetAll(
        int page, int pageSize,
        string? buscar, EstadoPedidoEnum? estado, DateTime? fecha)
        => await BuildFiltroQuery(buscar, estado, fecha)
            .OrderBy(p => p.id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<int> Count(string? buscar, EstadoPedidoEnum? estado, DateTime? fecha)
        => await BuildFiltroQuery(buscar, estado, fecha).CountAsync();

    public async Task<PedidoModel?> GetByIdConDetalle(int id)
        => await _context.Pedidos
            .Include(p => p.Mesero)
            .Include(p => p.Detalles)
                .ThenInclude(d => d.Plato)
            .FirstOrDefaultAsync(p => p.id == id);

    public async Task<List<PedidoModel>> GetTodos()
        => await _context.Pedidos.ToListAsync();

    public async Task<DetallePedidoModel> AddDetalle(DetallePedidoModel detalle)
    {
        await _context.DetallePedidos.AddAsync(detalle);
        return detalle;
    }

    public async Task SaveChanges()
        => await _context.SaveChangesAsync();


    private IQueryable<PedidoModel> BuildFiltroQuery(
        string? buscar, EstadoPedidoEnum? estado, DateTime? fecha)
    {
        var query = _context.Pedidos
            .Include(p => p.Mesero)
            .Include(p => p.Mesa)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            var txt = buscar.Trim();
            query = query.Where(p =>
                p.correlativo.Contains(txt) ||
                (p.dniCliente != null && p.dniCliente.Contains(txt)));
        }

        if (estado.HasValue)
            query = query.Where(p => p.estado == MapearEstado(estado.Value));

        if (fecha.HasValue)
        {
            var soloFecha = fecha.Value.Date;
            query = query.Where(p => p.fecha.Date == soloFecha);
        }

        return query;
    }

    private static string MapearEstado(EstadoPedidoEnum estado) => estado switch
    {
        EstadoPedidoEnum.Pendiente => "Pendiente",
        EstadoPedidoEnum.EnProceso => "En proceso",
        EstadoPedidoEnum.Entregado => "Entregado",
        _                         => "Pendiente"
    };
}
