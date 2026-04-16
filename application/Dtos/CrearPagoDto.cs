using System;
using System.ComponentModel.DataAnnotations;

namespace application.Dtos;

public record CrearPagoDto(
    [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un pedido.")] int pedidoId,
    [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un cajero.")] int usuarioId,
    [Range(typeof(double), "0.01", "999999999", ErrorMessage = "El monto debe ser mayor a 0.00.")] double monto,
    DateTime fecha,
    [Required(ErrorMessage = "Debes seleccionar un metodo de pago.")]
    [StringLength(20)] string metodoPago
);