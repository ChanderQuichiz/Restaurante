using application.Dtos;
using application.Models;
using application.Repositories;

namespace application.Services;

public class UserService : IUserService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private const int PageSize = 10;

    public UserService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<UserDto?> crearUser(CreateUserDto createUserDto)
    {
        var user = new UsuarioModel
        {
            nombre          = createUserDto.nombre,
            email           = createUserDto.email,
            contrasena      = createUserDto.contrasena,
            rol             = createUserDto.rol,
            estado          = createUserDto.estado,
            fechaExpiracion = createUserDto.fechaExpiracion,
            dni             = createUserDto.dni
        };

        user = await _usuarioRepository.Add(user); 
        return MapearDto(user);
    }

    public async Task<UserDto?> actualizarUser(UserDto userDto)
    {
        var user = await _usuarioRepository.GetById(userDto.id);
        if (user == null) return null;

        user.nombre          = userDto.nombre;
        user.email           = userDto.email;
        user.contrasena      = userDto.contrasena;
        user.rol             = userDto.rol;
        user.estado          = userDto.estado;
        user.fechaExpiracion = userDto.fechaExpiracion;
        user.dni             = userDto.dni;

        await _usuarioRepository.Update();
        return MapearDto(user);
    }

    public async Task<UserDto?> obtenerUserPorId(int id)
    {
        var user = await _usuarioRepository.GetById(id);
        return user == null ? null : MapearDto(user);
    }

    public async Task<bool> eliminarUser(int id)
    {
        var user = await _usuarioRepository.GetById(id);
        if (user == null) return false;

        user.estado = "Inactivo";
        await _usuarioRepository.Update();
        return true;
    }

    public async Task<List<UserDto>> obtenerUsers(int page = 1, FilterUserDto? filtro = null)
    {
        page = page < 1 ? 1 : page;
        var users = await _usuarioRepository.GetAll(page, PageSize, filtro);
        return users.Select(MapearDto).ToList();
    }

    public async Task<int> contarUsers(FilterUserDto? filtro = null)
        => await _usuarioRepository.Count(filtro);

    public async Task<UserVM> obtenerUserVM(int page = 1, FilterUserDto? filtro = null)
    {
        page = page < 1 ? 1 : page;

        var users      = await obtenerUsers(page, filtro);
        var total      = await contarUsers(filtro);
        var totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)PageSize));

        return new UserVM(users, page, totalPages, filtro ?? new FilterUserDto(null, null, null, page));
    }


    private static UserDto MapearDto(UsuarioModel u) => new(
        u.id, u.correlativo, u.nombre, u.email,
        u.contrasena, u.rol, u.estado, u.fechaExpiracion, u.dni
    );
}
