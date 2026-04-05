using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace application.Models;

[Table("pedidos")]
public class PedidoModel
{
    [Key]
    public int id { get; set; }

    [Required]
    [Column("fecha")]
    public DateTime fecha { get; set; }

    [Required]
    [Column("total")]
    public double total { get; set; }

    [Required]
    [MaxLength(30)]
    [Column("estado")]
    public string estado { get; set; } = string.Empty;

    [ForeignKey(nameof(Usuario))]
    [Column("usuario_id")]
    public int usuarioId { get; set; }

    [ForeignKey(nameof(Mesa))]
    [Column("mesa_id")]
    public int mesaId { get; set; }

    public UsuarioModel Usuario { get; set; } = null!;

    public MesaModel Mesa { get; set; } = null!;

}