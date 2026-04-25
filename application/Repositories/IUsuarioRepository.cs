using application.Dtos;
using application.Models;

namespace application.Repositories;

public interface IUsuarioRepository
{
    Task<UsuarioModel?> GetById(int id);
    Task<UsuarioModel> Add(UsuarioModel usuario);
    Task Update();
    Task Delete(UsuarioModel usuario);

    Task<List<UsuarioModel>> GetAll(int page, int pageSize, FilterUserDto? filtro);
    Task<int> Count(FilterUserDto? filtro);
    Task<UsuarioModel?> GetByEmailAndPassword(string email, string password);
    Task<List<UsuarioModel>> GetActivosByRol(string rol);
    Task<List<UsuarioModel>> GetActivos();
}
