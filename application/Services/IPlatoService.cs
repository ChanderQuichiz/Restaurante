using System;
using System.Runtime.CompilerServices;
using application.Dtos;

namespace application.Services;

public interface IPlatoService
{
    public  Task<PlatoDto?> crearPlatoDto(CrearPlatoDto crearPlatoDto);
    public Task<PlatoDto?> actualizarPlatoDto(PlatoDto actualizarPlatoDto);
    public Task<PlatoDto?> obtenerPlatoPorId(int id);
    public Task<bool> eliminarPlato(int id);
    public Task<List<PlatoDto>> obtenerPlatos(int page = 1, FiltrarPlatoDto? filtro = null);
    public Task<int> contarPlatos(FiltrarPlatoDto? filtro = null);
    public Task<PlatoVM> obtenerPlatoVM(int page = 1, FiltrarPlatoDto? filtro = null);


}
