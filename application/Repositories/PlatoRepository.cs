using application.Data;
using application.Dtos;
using application.Models;
using Microsoft.EntityFrameworkCore;

namespace application.Repositories;

public class PlatoRepository : IPlatoRepository
{
    private readonly DbAppContext _context;

    public PlatoRepository(DbAppContext context)
    {
        _context = context;
    }


    public async Task<PlatoModel?> GetById(int id)
        => await _context.Platos.FirstOrDefaultAsync(p => p.id == id);

    public async Task<PlatoModel> Add(PlatoModel plato)
    {
        await _context.Platos.AddAsync(plato);
        await _context.SaveChangesAsync();          

        plato.correlativo = $"PLA-{plato.id:D3}";
        await _context.SaveChangesAsync();         

        return plato;
    }

    public async Task Update()
        => await _context.SaveChangesAsync();

    public async Task Delete(PlatoModel plato)
    {
        _context.Platos.Remove(plato);
        await _context.SaveChangesAsync();
    }


    public async Task<List<PlatoModel>> GetAll(int page, int pageSize, FiltrarPlatoDto? filtro)
        => await BuildFiltroQuery(filtro)
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<int> Count(FiltrarPlatoDto? filtro)
        => await BuildFiltroQuery(filtro).CountAsync();

    public async Task<List<PlatoModel>> GetActivos()
        => await _context.Platos
            .Where(p => p.estado == "Activo")
            .OrderBy(p => p.nombre)
            .ToListAsync();



    private IQueryable<PlatoModel> BuildFiltroQuery(FiltrarPlatoDto? filtro)
    {
        var query = _context.Platos.AsQueryable();

        if (filtro == null) return query;

        if (!string.IsNullOrWhiteSpace(filtro.buscar))
        {
            var txt = filtro.buscar.Trim();
            query = query.Where(p => p.correlativo.Contains(txt) || p.nombre.Contains(txt));
        }

        if (filtro.categoria.HasValue)
            query = query.Where(p => p.categoria == filtro.categoria.Value.ToString());

        if (filtro.estado.HasValue)
            query = query.Where(p => p.estado == filtro.estado.Value.ToString());

        return query;
    }
}
