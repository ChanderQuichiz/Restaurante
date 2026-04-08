using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace application.Models;

[Table("usuarios")]
public class UsuarioModel
{
    [Key]
    public int id { get; set; }

    [Column("nombre")]
    public string nombre { get; set; } = string.Empty;

    [Column("email")]
    public string email { get; set; } = string.Empty;

    [Column("contrasena")]
    public string contrasena { get; set; } = string.Empty;

    [Column("rol")]
    public string rol { get; set; } = string.Empty;

    [Column("estado")]
    public string estado { get; set; } = string.Empty;

    [Column("fecha_expiracion")]
    public DateTime fechaExpiracion { get; set; }

    [Column("dni")]
    public string? dni { get; set; }
}