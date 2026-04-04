using System.ComponentModel.DataAnnotations;

namespace application.Dtos;

public record class CrearPlatoDto(
   [Required] string nombre,
   [Required] double precio,
   [Required] string categoria,
    [Required]string estado
);
