using application.Dtos;
using application.Models;

namespace application.Utils;

public class PagoMapper
{
    public static PagoDto ToPagoDto(PagoModel model)
    {
        return new PagoDto(
            id: model.id,
            pedidoId: model.pedidoId,
            usuarioId: model.usuarioId,
            monto: model.monto,
            fecha: model.fecha,
            metodoPago: model.metodoPago,
            cajeroNombre: model.Usuario != null ? model.Usuario.nombre : string.Empty
        );
    }

    public static PagoModel ToPagoModel(CrearPagoDto crearPagoDto)
    {
        return new PagoModel
        {
            pedidoId = crearPagoDto.pedidoId,
            usuarioId = crearPagoDto.usuarioId,
            monto = crearPagoDto.monto,
            fecha = crearPagoDto.fecha,
            metodoPago = crearPagoDto.metodoPago
        };
    }


}