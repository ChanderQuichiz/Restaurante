using application.Dtos;
using application.Models;

namespace application.Repositories;

public interface IMesaRepository
{
    Task<MesaModel?> GetById(int id);
    Task<MesaModel> Add(MesaModel mesa);
    Task Update();
    Task Delete(MesaModel mesa);

    Task<List<MesaModel>> GetAll(int page, int pageSize, FiltrarMesaDto? filtro);
    Task<int> Count(FiltrarMesaDto? filtro);
}
