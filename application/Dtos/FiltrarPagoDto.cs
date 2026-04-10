using System;

namespace application.Dtos;

public record class FiltrarPagoDto(
    string? buscar,
    string? metodo,
    DateTime? fecha,
    int page = 1
);