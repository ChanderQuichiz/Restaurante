using System;
using System.ComponentModel.DataAnnotations;

namespace application.Dtos;

public record PlatoDto(
    [Required] int id,
    [Required] string nombre,
    [Required] double precio,
    [Required] string categoria,
    [Required] string estado
);
