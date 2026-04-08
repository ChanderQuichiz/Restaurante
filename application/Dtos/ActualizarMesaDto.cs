using System.ComponentModel.DataAnnotations;

namespace application.Dtos;

public record ActualizarMesaDto(
   [Required] int id,
   [Required] int numeroPiso,
   [Required] int capacidad,
   [Required] string estado
);
