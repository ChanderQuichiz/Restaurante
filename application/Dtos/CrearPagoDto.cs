using System;
using System.ComponentModel.DataAnnotations;

namespace application.Dtos;

public record CrearPagoDto(
    [Range(1, int.MaxValue)] int pedidoId,
    [Range(1, int.MaxValue)] int usuarioId,
    [Range(typeof(double), "0.01", "1.7976931348623157E+308")] double monto,
    DateTime fecha,
    [Required] [StringLength(20)] string metodoPago
);