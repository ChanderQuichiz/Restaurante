using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace application.Dtos;

public record CrearMesaDto(
  [Required]  int numeroPiso,
   [Required] int capacidad,
    [Required] string estado
);