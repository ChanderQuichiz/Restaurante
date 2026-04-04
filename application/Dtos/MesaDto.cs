using System;
using System.ComponentModel.DataAnnotations;

namespace application.Dtos;

public record MesaDto
(
    [Required] int id,
   [Required] int numeroPiso,
   [Required] int capacidad,
   [Required] string estado
);
