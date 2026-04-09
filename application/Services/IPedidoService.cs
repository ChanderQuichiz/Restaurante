using application.Dtos;
using application.Enums;
using application.Models;

namespace application.Services;

public interface IPedidoService
{
    Task<PedidoVM> obtenerPedidosVM(int page = 1, string? buscar = null, EstadoPedidoEnum? estado = null, DateTime? fecha = null);
    Task<PedidoDto> crearPedido(CrearPedidoDto crearPedidoDto);
    Task<PedidoDetalleVM?> obtenerDetallePedido(int pedidoId);
    Task<List<UsuarioModel>> obtenerMeserosActivos();
    Task<List<MesaModel>> obtenerMesasDisponibles();
    Task<List<PlatoModel>> obtenerPlatosActivos();
}