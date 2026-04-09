using System;
using System.ComponentModel.DataAnnotations;
using application.Enums;

namespace application.Dtos;

public record PlatoDto(
    [Required] int id,
    [Required] string nombre,
    [Required] double precio,
    [Required] CategoriaPlatoEnum categoria,
    [Required] EstadoPlatoEnum estado
);
