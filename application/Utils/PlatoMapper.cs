using System;
using application.Dtos;
using application.Enums;
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
            categoria: Enum.TryParse<CategoriaPlatoEnum>(model.categoria, true, out var categoria)
                ? categoria
                : CategoriaPlatoEnum.Fondo,
            estado: Enum.TryParse<EstadoPlatoEnum>(model.estado, true, out var estado)
                ? estado
                : EstadoPlatoEnum.ACTIVO
        );
    }
    public static PlatoModel ToPlatoModel(CrearPlatoDto crearPlatoDto)
    {
        return new PlatoModel
        {
            nombre = crearPlatoDto.nombre,
            precio = crearPlatoDto.precio,
            categoria = crearPlatoDto.categoria.ToString(),
            estado = crearPlatoDto.estado.ToString()
        };
    }
}
