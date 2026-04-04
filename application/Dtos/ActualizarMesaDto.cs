using System.ComponentModel.DataAnnotations;

namespace application.Dtos;

public record class ActualizarMesaDto(
   [Required] int id,
   [Required] int numeroPiso,
   [Required] int capacidad,
   [Required] string estado
);
