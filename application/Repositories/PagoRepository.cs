using application.Data;
using application.Dtos;
using application.Models;
using Microsoft.EntityFrameworkCore;

namespace application.Repositories;

public class PagoRepository : IPagoRepository
{
    private readonly DbAppContext _context;

    public PagoRepository(DbAppContext context)
    {
        _context = context;
    }



    public async Task<PagoModel?> GetById(int id)
        => await _context.Pagos.FirstOrDefaultAsync(p => p.id == id);

    public async Task<PagoModel> Add(PagoModel pago)
    {
        await _context.Pagos.AddAsync(pago);
        await _context.SaveChangesAsync();         

        pago.correlativo = $"PAG-{pago.id:D3}";
        await _context.SaveChangesAsync();          

        return pago;
    }

    public async Task Update()
        => await _context.SaveChangesAsync();

    public async Task Delete(PagoModel pago)
    {
        _context.Pagos.Remove(pago);
        await _context.SaveChangesAsync();
    }


    public async Task<List<PagoModel>> GetAll(int page, int pageSize, FiltrarPagoDto? filtro)
        => await BuildFiltroQuery(filtro)
            .Include(p => p.Usuario)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<int> Count(FiltrarPagoDto? filtro)
        => await BuildFiltroQuery(filtro).CountAsync();

    public async Task LoadUsuario(PagoModel pago)
        => await _context.Entry(pago).Reference(p => p.Usuario).LoadAsync();



    private IQueryable<PagoModel> BuildFiltroQuery(FiltrarPagoDto? filtro)
    {
        var query = _context.Pagos.AsQueryable();

        if (filtro == null) return query;

        if (!string.IsNullOrWhiteSpace(filtro.buscar))
        {
            var txt = filtro.buscar.Trim();
            query = query.Where(p =>
                p.correlativo.Contains(txt) ||
                p.pedidoId.ToString().Contains(txt));
        }

        if (!string.IsNullOrWhiteSpace(filtro.metodo))
            query = query.Where(p => p.metodoPago == filtro.metodo);

        if (filtro.fecha.HasValue)
        {
            var fecha = filtro.fecha.Value.Date;
            query = query.Where(p => p.fecha.Date == fecha);
        }

        return query;
    }
}
