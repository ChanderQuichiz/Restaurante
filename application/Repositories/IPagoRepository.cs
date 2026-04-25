using application.Dtos;
using application.Models;

namespace application.Repositories;

public interface IPagoRepository
{
    Task<PagoModel?> GetById(int id);
    Task<PagoModel> Add(PagoModel pago);
    Task Update();
    Task Delete(PagoModel pago);

    Task<List<PagoModel>> GetAll(int page, int pageSize, FiltrarPagoDto? filtro);
    Task<int> Count(FiltrarPagoDto? filtro);
    Task LoadUsuario(PagoModel pago);
}
