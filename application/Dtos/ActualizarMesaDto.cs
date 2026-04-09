using System.ComponentModel.DataAnnotations;
using application.Enums;

namespace application.Dtos;

public record ActualizarMesaDto(
   [Required] int id,
   [Required] int numeroPiso,
   [Required] int capacidad,
   [Required] EstadoMesaEnum estado
);
