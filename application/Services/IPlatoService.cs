using System;
using application.Dtos;

namespace application.Services;

public interface IPlatoService
{
    public PlatoDto? crearPlatoDto(CrearPlatoDto crearPlatoDto);
    public PlatoDto? actualizarPlatoDto(PlatoDto actualizarPlatoDto);
}
