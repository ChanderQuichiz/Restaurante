using application.Data;
using application.Dtos;
using application.Models;
using application.Utils;
using Microsoft.EntityFrameworkCore;

namespace application.Services;

public class PagoService : IPagoService
{
    private readonly DbAppContext context;
    private const int PageSize = 10;

    public PagoService(DbAppContext context)
    {
        this.context = context;
    }

    public async Task<PagoVM> obtenerPagoVM(int page = 1, FiltrarPagoDto? filtro = null)
    {
        page = page < 1 ? 1 : page;

        var pagos = await obtenerPagos(page, filtro);
        var totalPagos = await contarPagos(filtro);
        var totalPages = (int)Math.Ceiling(totalPagos / (double)PageSize);
        totalPages = totalPages == 0 ? 1 : totalPages;

        return new PagoVM(pagos, page, totalPages, filtro ?? new FiltrarPagoDto(null, null, null, page));
    }

    public async Task<List<PagoDto>> obtenerPagos(int page = 1, FiltrarPagoDto? filtro = null)
    {
        page = page < 1 ? 1 : page;

        var query = context.Pagos
            .Include(p => p.Usuario)
            .AsQueryable();

        query = AplicarFiltros(query, filtro);

        var lista = await query
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        return lista.Select(PagoMapper.ToPagoDto).ToList();
    }

    public async Task<int> contarPagos(FiltrarPagoDto? filtro = null)
    {
        var query = context.Pagos.AsQueryable();

        query = AplicarFiltros(query, filtro);

        return await query.CountAsync();
    }

    public async Task<PagoDto?> crearPagoDto(CrearPagoDto crearPagoDto)
    {
        var pago = PagoMapper.ToPagoModel(crearPagoDto);

        if (pago.fecha == default)
        {
            pago.fecha = DateTime.Now;
        }

        await context.Pagos.AddAsync(pago);
        await context.SaveChangesAsync();

        await context.Entry(pago).Reference(p => p.Usuario).LoadAsync();

        return PagoMapper.ToPagoDto(pago);
    }



    public async Task<List<UsuarioModel>> obtenerCajerosActivos()
    {
        var cajeros = await context.Usuarios
            .Where(u => u.estado == "Activo" && u.rol == "Cajero")
            .OrderBy(u => u.nombre)
            .ToListAsync();

        if (cajeros.Count == 0)
        {
            cajeros = await context.Usuarios
                .Where(u => u.estado == "Activo")
                .OrderBy(u => u.nombre)
                .ToListAsync();
        }

        return cajeros;
    }

    public async Task<List<PedidoModel>> obtenerPedidos()
    {
        return await context.Pedidos
            .ToListAsync();
    }

    private static IQueryable<PagoModel> AplicarFiltros(IQueryable<PagoModel> query, FiltrarPagoDto? filtro)
    {
        if (filtro == null)
        {
            return query;
        }

        if (!string.IsNullOrWhiteSpace(filtro.buscar))
        {
            var txt = filtro.buscar.Trim();
            query = query.Where(p => p.pedidoId.ToString().Contains(txt));
        }

        if (!string.IsNullOrWhiteSpace(filtro.metodo))
        {
            query = query.Where(p => p.metodoPago == filtro.metodo);
        }

        if (filtro.fecha.HasValue)
        {
            var fecha = filtro.fecha.Value.Date;
            query = query.Where(p => p.fecha.Date == fecha);
        }

        return query;
    }
}