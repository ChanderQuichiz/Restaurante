using System;
using application.Dtos;

namespace application.Services;

public interface IMesaService
{
    public Task<MesaDto?> crearMesaDto(CrearMesaDto crearMesaDto); 
    public Task<MesaDto?> actualizarMesaDto(MesaDto actualizarMesaDto);
    public Task<MesaDto?> obtenerMesaPorId(int id);
    public Task<bool> eliminarMesa(int id);
    public Task<List<MesaDto>> obtenerMesas(int page = 1, FiltrarMesaDto? filtro = null);
    public Task<int> contarMesas(FiltrarMesaDto? filtro = null);
    public Task<MesaVM> obtenerMesaVM(int page = 1, FiltrarMesaDto? filtro = null);
}
