using application.Data;
using application.Dtos;
using application.Models;
using Microsoft.EntityFrameworkCore;

namespace application.Repositories;

public class MesaRepository : IMesaRepository
{
    private readonly DbAppContext _context;

    public MesaRepository(DbAppContext context)
    {
        _context = context;
    }

    // ── CRUD base ──────────────────────────────────────────────────────────────

    public async Task<MesaModel?> GetById(int id)
        => await _context.Mesas.FirstOrDefaultAsync(m => m.id == id);

    public async Task<MesaModel> Add(MesaModel mesa)
    {
        await _context.Mesas.AddAsync(mesa);
        await _context.SaveChangesAsync();          // EF genera el id aquí

        mesa.correlativo = $"MES-{mesa.id:D3}";
        await _context.SaveChangesAsync();          // persiste el correlativo

        return mesa;
    }

    public async Task Update()
        => await _context.SaveChangesAsync();

    public async Task Delete(MesaModel mesa)
    {
        _context.Mesas.Remove(mesa);
        await _context.SaveChangesAsync();
    }

    // ── Consultas ──────────────────────────────────────────────────────────────

    public async Task<List<MesaModel>> GetAll(int page, int pageSize, FiltrarMesaDto? filtro)
        => await BuildFiltroQuery(filtro)
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<int> Count(FiltrarMesaDto? filtro)
        => await BuildFiltroQuery(filtro).CountAsync();

    // ── Helpers ────────────────────────────────────────────────────────────────

    private IQueryable<MesaModel> BuildFiltroQuery(FiltrarMesaDto? filtro)
    {
        var query = _context.Mesas.AsQueryable();

        if (filtro == null) return query;

        if (!string.IsNullOrWhiteSpace(filtro.codigo))
            query = query.Where(m => m.correlativo.Contains(filtro.codigo.Trim()));

        if (filtro.piso != null)
            query = query.Where(m => m.numeroPiso == (int)filtro.piso.Value);

        if (filtro.estado != null)
            query = query.Where(m => m.estado == filtro.estado.ToString());

        return query;
    }
}
