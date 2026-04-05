using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace application.Models;

[Table("solicitudes")]
public class SolicitudModel
{
    [Key]
    public int id { get; set; }

    [Required]
    [Column("fecha")]
    public DateTime fecha { get; set; }

    [Required]
    [MaxLength(30)]
    [Column("estado")]
    public string estado { get; set; } = string.Empty;

    [Required]
    [Column("contrato", TypeName = "longblob")]
    public byte[] contrato { get; set; } = Array.Empty<byte>();

    [ForeignKey(nameof(Usuario))]
    [Column("usuario_id")]
    public int usuarioId { get; set; }

    public UsuarioModel Usuario { get; set; } = null!;
}