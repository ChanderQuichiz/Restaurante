using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace application.Models;

[Table("pagos")]
public class PagoModel
{
    [Key]
    public int id { get; set; }

    [Column("pedido_id")]
    public int pedidoId { get; set; }

    [Column("usuario_id")]
    public int usuarioId { get; set; }

    [Column("monto")]
    public double monto { get; set; }

    [Column("fecha")]
    public DateTime fecha { get; set; }

    [Column("metodo_pago")]
    public string metodoPago { get; set; } = string.Empty;

    [ForeignKey(nameof(pedidoId))]
    public PedidoModel Pedido { get; set; } = null!;

    [ForeignKey(nameof(usuarioId))]
    public UsuarioModel Usuario { get; set; } = null!;
}