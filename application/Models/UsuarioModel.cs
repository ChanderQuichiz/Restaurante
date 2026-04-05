using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace application.Models;

[Table("usuarios")]
public class UsuarioModel
{
    [Key]
    public int id { get; set; }

    [Required]
    [StringLength(120)]
    [Column("nombre")]
    public string nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(180)]
    [Column("email")]
    public string email { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [Column("contrasena")]
    public string contrasena { get; set; } = string.Empty;

    [Required]
    [MaxLength(40)]
    [Column("rol")]
    public string rol { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    [Column("estado")]
    public string estado { get; set; } = string.Empty;

    [Required]
    [Column("fecha_expiracion")]
    public DateTime fechaExpiracion { get; set; }
}