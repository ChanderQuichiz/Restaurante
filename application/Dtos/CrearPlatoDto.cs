using System.ComponentModel.DataAnnotations;
using application.Enums;

namespace application.Dtos;

public record CrearPlatoDto(
   [Required] string nombre,
   [Required] double precio,
   [Required] CategoriaPlatoEnum categoria,
    [Required] EstadoPlatoEnum estado
);
