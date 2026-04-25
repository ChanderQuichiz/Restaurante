using application.Enums;
using application.Models;

namespace application.Repositories;

public interface IPedidoRepository
{
    Task<PedidoModel?> GetById(int id);
    Task<PedidoModel> Add(PedidoModel pedido);
    Task Update();
    Task Delete(PedidoModel pedido);

    Task<List<PedidoModel>> GetAll(int page, int pageSize, string? buscar, EstadoPedidoEnum? estado, DateTime? fecha);
    Task<int> Count(string? buscar, EstadoPedidoEnum? estado, DateTime? fecha);
    Task<PedidoModel?> GetByIdConDetalle(int id);
    Task<List<PedidoModel>> GetTodos();
    Task<DetallePedidoModel> AddDetalle(DetallePedidoModel detalle);
    Task SaveChanges();
}
