using application.Dtos;

namespace application.Services;

public interface IUserService
{
    Task<UserVM> obtenerUserVM(int page = 1, FilterUserDto? filtro = null);
    Task<List<UserDto>> obtenerUsers(int page = 1, FilterUserDto? filtro = null);
    Task<int> contarUsers(FilterUserDto? filtro = null);
    Task<UserDto?> crearUser(CreateUserDto createUserDto);
    Task<UserDto?> actualizarUser(UserDto userDto);
    Task<UserDto?> obtenerUserPorId(int id);
    Task<bool> eliminarUser(int id);
}
