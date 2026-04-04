using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace application.Models;

[Table("platos")]
public class PlatoModel
{
    [Key]
    public int id {get; set;}
    [Required]
    public string nombre {get; set;}
    [Required]
    public double precio {get; set;}
    [Required]
    public string categoria {get; set;}
    [Required]
    public string estado   {get; set;}
}
