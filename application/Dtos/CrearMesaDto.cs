using System.ComponentModel.DataAnnotations;
using application.Enums;

namespace application.Dtos;

public record CrearMesaDto(
  [Required]  int numeroPiso,
   [Required] int capacidad,
  [Required] EstadoMesaEnum estado
);