using System;
using application.Dtos;
using application.Enums;
using application.Models;

namespace application.Utils;

public class MesaMapper
{
    public static MesaDto ToMesaDto(MesaModel model)
    {
        return new MesaDto(
            id: model.id,
            numeroPiso: model.numeroPiso,
            capacidad: model.capacidad,
            estado: Enum.TryParse<EstadoMesaEnum>(model.estado, true, out var estado)
                ? estado
                : EstadoMesaEnum.LIBRE
        );
    }
    public static MesaModel ToMesaModel(CrearMesaDto crearMesaDto)
    {
        return new MesaModel
        {
            
            numeroPiso = crearMesaDto.numeroPiso,
            capacidad = crearMesaDto.capacidad,
            estado = crearMesaDto.estado.ToString()
        };
    }
}
