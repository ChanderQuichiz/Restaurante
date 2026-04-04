using System;
using application.Dtos;
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
            estado: model.estado
        );
    }
    public static MesaModel ToMesaModel(CrearMesaDto crearMesaDto)
    {
        return new MesaModel
        {
            
            numeroPiso = crearMesaDto.numeroPiso,
            capacidad = crearMesaDto.capacidad,
            estado = crearMesaDto.estado
        };
    }
}
