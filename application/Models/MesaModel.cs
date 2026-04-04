using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace application.Models;

[Table("mesas")]
public class MesaModel
{
    [Key]
    public int id {get; set;}
    [Required]
    public int numeroPiso {get; set;}
    [Required]
    public int capacidad {get; set;}
    [Required]
    public string estado {get; set;}
}
