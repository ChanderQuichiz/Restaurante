using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace application.Models;

[Table("pagos")]
public class PagoModel
{
    [Key]
    public int id { get; set; }

    [Required]
    [Column("fecha")]
    public DateTime fecha { get; set; }

    [Required]
    [Column("monto")]
    public double monto { get; set; }

    [Required]
    [MaxLength(40)]
    [Column("metodo_pago")]
    public string metodoPago { get; set; } = string.Empty;

    [ForeignKey(nameof(Pedido))]
    [Column("pedido_id")]
    public int pedidoId { get; set; }

    [ForeignKey(nameof(Usuario))]
    [Column("usuario_id")]
    public int usuarioId { get; set; }

    public PedidoModel Pedido { get; set; } = null!;

    public UsuarioModel Usuario { get; set; } = null!;
}