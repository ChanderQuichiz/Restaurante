using System.ComponentModel.DataAnnotations;

namespace application.Dtos;

public record DetallePedidoDto(
    [Required] int id,
    [Required] int pedidoId,
    [Required] int platoId,
    [Required] int cantidad,
    [Required] double precioUnitario,
    [Required] string platoNombre
);
