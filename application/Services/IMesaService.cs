using System;
using application.Dtos;

namespace application.Services;

public interface IMesaService
{
    public Task<MesaDto?> crearMesaDto(CrearMesaDto crearMesaDto); 
    public Task<MesaDto?> actualizarMesaDto(MesaDto actualizarMesaDto);
    public Task<MesaDto?> obtenerMesaPorId(int id);
    public Task<bool> eliminarMesa(int id);
    public Task<List<MesaDto>> obtenerMesas(int page = 1);
    public Task<int> contarMesas();
    public Task<MesaVM> obtenerMesaVM(int page = 1);
}
