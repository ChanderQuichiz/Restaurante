using System;
using application.Dtos;
using application.Models;

namespace application.Utils;

public class PlatoMapper
{
    public static PlatoDto ToPlatoDto(PlatoModel model)
    {
        return new PlatoDto(
            id: model.id,
            nombre: model.nombre,
            precio: model.precio,
            categoria: model.categoria,
            estado: model.estado
        );
    }
    public static PlatoModel ToPlatoModel(CrearPlatoDto crearPlatoDto)
    {
        return new PlatoModel
        {
            nombre = crearPlatoDto.nombre,
            precio = crearPlatoDto.precio,
            categoria = crearPlatoDto.categoria,
            estado = crearPlatoDto.estado
        };
    }
}
