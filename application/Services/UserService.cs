using application.Data;
using application.Dtos;
using application.Models;
using Microsoft.EntityFrameworkCore;

namespace application.Services;

public class UserService : IUserService
{
    private readonly DbAppContext context;
    private const int PageSize = 10;

    public UserService(DbAppContext context)
    {
        this.context = context;
    }

    public async Task<UserVM> obtenerUserVM(int page = 1, FilterUserDto? filtro = null)
    {
        page = page < 1 ? 1 : page;

        var users = await obtenerUsers(page, filtro);
        var totalUsers = await contarUsers(filtro);
        var totalPages = (int)Math.Ceiling(totalUsers / (double)PageSize);
        totalPages = totalPages == 0 ? 1 : totalPages;

        return new UserVM(users, page, totalPages, filtro ?? new FilterUserDto(null, null, null, page));
    }

    public async Task<List<UserDto>> obtenerUsers(int page = 1, FilterUserDto? filtro = null)
    {
        page = page < 1 ? 1 : page;

        var query = context.Usuarios.AsQueryable();
        query = aplicarFiltros(query, filtro);

        var users = await query
            .OrderBy(u => u.id)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        return users.Select(mapearDto).ToList();
    }

    public async Task<int> contarUsers(FilterUserDto? filtro = null)
    {
        var query = context.Usuarios.AsQueryable();
        query = aplicarFiltros(query, filtro);

        return await query.CountAsync();
    }

    public async Task<UserDto?> crearUser(CreateUserDto createUserDto)
    {
        var user = new UsuarioModel
        {
            nombre = createUserDto.nombre,
            email = createUserDto.email,
            contrasena = createUserDto.contrasena,
            rol = createUserDto.rol,
            estado = createUserDto.estado,
            fechaExpiracion = createUserDto.fechaExpiracion,
            dni = createUserDto.dni
        };

        await context.Usuarios.AddAsync(user);
        await context.SaveChangesAsync();

        return mapearDto(user);
    }

    public async Task<UserDto?> actualizarUser(UserDto userDto)
    {
        var user = await context.Usuarios.FirstOrDefaultAsync(u => u.id == userDto.id);
        if (user == null)
        {
            return null;
        }

        user.nombre = userDto.nombre;
        user.email = userDto.email;
        user.contrasena = userDto.contrasena;
        user.rol = userDto.rol;
        user.estado = userDto.estado;
        user.fechaExpiracion = userDto.fechaExpiracion;
        user.dni = userDto.dni;

        await context.SaveChangesAsync();

        return mapearDto(user);
    }

    public async Task<UserDto?> obtenerUserPorId(int id)
    {
        var user = await context.Usuarios.FirstOrDefaultAsync(u => u.id == id);
        return user == null ? null : mapearDto(user);
    }

    public async Task<bool> eliminarUser(int id)
    {
        var user = await context.Usuarios.FirstOrDefaultAsync(u => u.id == id);
        if (user == null)
        {
            return false;
        }

        user.estado = "Inactivo";
        await context.SaveChangesAsync();
        return true;
    }

    private static IQueryable<UsuarioModel> aplicarFiltros(IQueryable<UsuarioModel> query, FilterUserDto? filtro)
    {
        if (filtro == null)
        {
            return query;
        }

        if (!string.IsNullOrWhiteSpace(filtro.buscar))
        {
            var txt = filtro.buscar.Trim();
            query = query.Where(u =>
                u.nombre.Contains(txt) ||
                u.email.Contains(txt) ||
                (u.dni != null && u.dni.Contains(txt)));
        }

        if (!string.IsNullOrWhiteSpace(filtro.rol))
        {
            query = query.Where(u => u.rol == filtro.rol);
        }

        if (!string.IsNullOrWhiteSpace(filtro.estado))
        {
            query = query.Where(u => u.estado == filtro.estado);
        }

        return query;
    }

    private static UserDto mapearDto(UsuarioModel user)
    {
        return new UserDto(
            user.id,
            user.nombre,
            user.email,
            user.contrasena,
            user.rol,
            user.estado,
            user.fechaExpiracion,
            user.dni
        );
    }

}
