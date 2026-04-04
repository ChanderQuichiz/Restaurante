using System;
using application.Dtos;

namespace application.Services;

public interface IMesaService
{
    public MesaDto? crearMesaDto(CrearMesaDto crearMesaDto); 
    public MesaDto? actualizarMesaDto(MesaDto actualizarMesaDto);

}
