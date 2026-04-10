using System;
using System.ComponentModel.DataAnnotations;

namespace application.Dtos;

public record PagoDto(
    int id,
    int pedidoId,
    int usuarioId,
    double monto,
    DateTime fecha,
    [Required] [StringLength(20)] string metodoPago,
    string? cajeroNombre
);