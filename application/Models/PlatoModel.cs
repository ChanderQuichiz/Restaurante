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
    [StringLength(120)]
    public string nombre {get; set;} = string.Empty;

    [Required]
    public double precio {get; set;}

    [Required]
    [MaxLength(80)]
    public string categoria {get; set;} = string.Empty;

    [Required]
    [MaxLength(30)]
    public string estado   {get; set;} = string.Empty;
}
