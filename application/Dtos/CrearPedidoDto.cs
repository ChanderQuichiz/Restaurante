using System.ComponentModel.DataAnnotations;
using application.Enums;

namespace application.Dtos;

public record CrearPedidoDto(
    [Required] [Range(1, int.MaxValue)] int meseroId,
    [Required] [StringLength(20, MinimumLength = 8)] string dniCliente,
    [Required] [Range(1, int.MaxValue)] int mesaId,
    [Required] EstadoPedidoEnum estado,
    [Required] [MinLength(1)] List<int> platoIds,
    [Required] [MinLength(1)] List<int> cantidades,
    [Required] [MinLength(1)] List<double> precios
);