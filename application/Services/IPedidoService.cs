using application.Dtos;
using application.Models;

namespace application.Services;

public interface IPedidoService
{
    Task<List<PedidoDto>> obtenerPedidos(string? buscar, string? estado, DateTime? fecha);
    Task<PedidoDto> crearPedido(CrearPedidoDto crearPedidoDto);
    Task<PedidoDetalleVM?> obtenerDetallePedido(int pedidoId);
    Task<List<UsuarioModel>> obtenerMeserosActivos();
    Task<List<MesaModel>> obtenerMesasDisponibles();
    Task<List<PlatoModel>> obtenerPlatosActivos();
}