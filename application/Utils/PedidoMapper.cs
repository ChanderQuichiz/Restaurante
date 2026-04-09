using System;
using application.Dtos;
using application.Enums;
using application.Models;

namespace application.Utils;

public class PedidoMapper
{
    public static PedidoDto ToPedidoDto(PedidoModel model)
    {
        return new PedidoDto(
            id: model.id,
            meseroId: model.meseroId,
            mesaId: model.mesaId,
            fecha: model.fecha,
            total: model.total,
            estado: mapearEstadoPedido(model.estado),
            dniCliente: model.dniCliente,
            meseroNombre: model.Mesero != null ? model.Mesero.nombre : string.Empty
        );
    }

    public static PedidoModel ToPedidoModel(CrearPedidoDto crearPedidoDto)
    {
        return new PedidoModel
        {
            meseroId = crearPedidoDto.meseroId,
            dniCliente = crearPedidoDto.dniCliente,
            mesaId = crearPedidoDto.mesaId,
            estado = mapearEstadoPedidoTexto(crearPedidoDto.estado)
        };
    }

    public static DetallePedidoDto ToDetallePedidoDto(DetallePedidoModel model)
    {
        return new DetallePedidoDto(
            id: model.id,
            pedidoId: model.pedidoId,
            platoId: model.platoId,
            cantidad: model.cantidad,
            precioUnitario: model.precioUnitario,
            platoNombre:model.Plato.nombre 
        );
    }

    private static EstadoPedidoEnum mapearEstadoPedido(string estado)
    {
        return estado switch
        {
            "Pendiente" => EstadoPedidoEnum.Pendiente,
            "En proceso" => EstadoPedidoEnum.EnProceso,
            "Entregado" => EstadoPedidoEnum.Entregado,
            _ => EstadoPedidoEnum.Pendiente
        };
    }

    private static string mapearEstadoPedidoTexto(EstadoPedidoEnum estado)
    {
        return estado switch
        {
            EstadoPedidoEnum.Pendiente => "Pendiente",
            EstadoPedidoEnum.EnProceso => "En proceso",
            EstadoPedidoEnum.Entregado => "Entregado",
            _ => "Pendiente"
        };
    }
}