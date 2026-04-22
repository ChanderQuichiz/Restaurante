using System;
using System.ComponentModel.DataAnnotations;
using application.Enums;

namespace application.Dtos;

public record MesaDto
(
    [Required] int id,
   [Required] string correlativo,
   [Required] int numeroPiso,
   [Required] int capacidad,
    [Required] EstadoMesaEnum estado
);
