using application.Dtos;

namespace application.Services;

public interface IPlatoService
{
    Task<PlatoDto?> crearPlatoDto(CrearPlatoDto crearPlatoDto);
    Task<PlatoDto?> actualizarPlatoDto(PlatoDto actualizarPlatoDto);
    Task<PlatoDto?> obtenerPlatoPorId(int id);
    Task<bool> eliminarPlato(int id);
    Task<List<PlatoDto>> obtenerPlatos(int page = 1, FiltrarPlatoDto? filtro = null);
    Task<int> contarPlatos(FiltrarPlatoDto? filtro = null);
    Task<PlatoVM> obtenerPlatoVM(int page = 1, FiltrarPlatoDto? filtro = null);
}
