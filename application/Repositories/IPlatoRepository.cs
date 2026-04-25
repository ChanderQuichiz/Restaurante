using application.Dtos;
using application.Models;

namespace application.Repositories;

public interface IPlatoRepository
{
    Task<PlatoModel?> GetById(int id);
    Task<PlatoModel> Add(PlatoModel plato);
    Task Update();
    Task Delete(PlatoModel plato);

    Task<List<PlatoModel>> GetAll(int page, int pageSize, FiltrarPlatoDto? filtro);
    Task<int> Count(FiltrarPlatoDto? filtro);
    Task<List<PlatoModel>> GetActivos();
}
