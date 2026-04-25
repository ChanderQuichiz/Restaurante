using application.Data;
using application.Dtos;
using application.Models;
using Microsoft.EntityFrameworkCore;

namespace application.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly DbAppContext _context;

    public UsuarioRepository(DbAppContext context)
    {
        _context = context;
    }


    public async Task<UsuarioModel?> GetById(int id)
        => await _context.Usuarios.FirstOrDefaultAsync(u => u.id == id);

    public async Task<UsuarioModel> Add(UsuarioModel usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();          

        usuario.correlativo = $"USU-{usuario.id:D3}";
        await _context.SaveChangesAsync();          

        return usuario;
    }

    public async Task Update()
        => await _context.SaveChangesAsync();

    public async Task Delete(UsuarioModel usuario)
    {
        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
    }


    public async Task<List<UsuarioModel>> GetAll(int page, int pageSize, FilterUserDto? filtro)
        => await BuildFiltroQuery(filtro)
            .OrderBy(u => u.id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<int> Count(FilterUserDto? filtro)
        => await BuildFiltroQuery(filtro).CountAsync();

    public async Task<UsuarioModel?> GetByEmailAndPassword(string email, string password)
        => await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u =>
                u.email == email &&
                u.contrasena == password &&
                u.estado == "Activo");

    public async Task<List<UsuarioModel>> GetActivosByRol(string rol)
        => await _context.Usuarios
            .Where(u => u.estado == "Activo" && u.rol == rol)
            .OrderBy(u => u.nombre)
            .ToListAsync();

    public async Task<List<UsuarioModel>> GetActivos()
        => await _context.Usuarios
            .Where(u => u.estado == "Activo")
            .OrderBy(u => u.nombre)
            .ToListAsync();


    private IQueryable<UsuarioModel> BuildFiltroQuery(FilterUserDto? filtro)
    {
        var query = _context.Usuarios.AsQueryable();

        if (filtro == null) return query;

        if (!string.IsNullOrWhiteSpace(filtro.buscar))
        {
            var txt = filtro.buscar.Trim();
            query = query.Where(u =>
                u.correlativo.Contains(txt) ||
                u.nombre.Contains(txt) ||
                u.email.Contains(txt) ||
                (u.dni != null && u.dni.Contains(txt)));
        }

        if (!string.IsNullOrWhiteSpace(filtro.rol))
            query = query.Where(u => u.rol == filtro.rol);

        if (!string.IsNullOrWhiteSpace(filtro.estado))
            query = query.Where(u => u.estado == filtro.estado);

        return query;
    }
}
