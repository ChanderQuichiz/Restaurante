using application.Dtos;

namespace application.Services;

public interface IMesaService
{
    Task<MesaDto?> crearMesaDto(CrearMesaDto crearMesaDto);
    Task<MesaDto?> actualizarMesaDto(MesaDto actualizarMesaDto);
    Task<MesaDto?> obtenerMesaPorId(int id);
    Task<bool> eliminarMesa(int id);
    Task<List<MesaDto>> obtenerMesas(int page = 1, FiltrarMesaDto? filtro = null);
    Task<int> contarMesas(FiltrarMesaDto? filtro = null);
    Task<MesaVM> obtenerMesaVM(int page = 1, FiltrarMesaDto? filtro = null);
}
