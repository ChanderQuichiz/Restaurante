using application.Dtos;
using application.Models;

namespace application.Services;

public interface IPagoService
{
    Task<PagoVM> obtenerPagoVM(int page = 1, FiltrarPagoDto? filtro = null);
    Task<List<PagoDto>> obtenerPagos(int page = 1, FiltrarPagoDto? filtro = null);
    Task<int> contarPagos(FiltrarPagoDto? filtro = null);
    Task<PagoDto?> crearPagoDto(CrearPagoDto crearPagoDto);
    Task<List<UsuarioModel>> obtenerCajerosActivos();
    Task<List<PedidoModel>> obtenerPedidos();
}