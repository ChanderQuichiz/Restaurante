using System;
using System.ComponentModel.DataAnnotations;
using application.Enums;

namespace application.Dtos;

public record PedidoDto(
    [Required] int id,
    [Required] int meseroId,
    [Required] int mesaId,
    [Required] DateTime fecha,
    [Required] double total,
    [Required] EstadoPedidoEnum estado,
    [Required][StringLength(20)] string dniCliente,
    [Required] string meseroNombre
);