using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace application.Models;

[Table("mesas")]
public class MesaModel
{
    [Key]
    [Column("id")]
    public int id {get; set;}



    [Required]
    [Column("numero_piso")]
    public int numeroPiso {get; set;}

    [Required]
    [Column("capacidad")]
    public int capacidad {get; set;}

    [Required]
    [MaxLength(30)]
    [Column("estado")]
    public string estado {get; set;} = string.Empty;


}
