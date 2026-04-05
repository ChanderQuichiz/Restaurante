using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace application.Models;

[Table("detalle_pedido")]
public class DetallePedidoModel
{
    [Key]
    public int id { get; set; }

    [Required]
    [Column("cantidad")]
    public int cantidad { get; set; }

    [Required]
    [Column("precio_unitario")]
    public double precioUnitario { get; set; }

    [ForeignKey(nameof(Pedido))]
    [Column("pedido_id")]
    public int pedidoId { get; set; }

    [ForeignKey(nameof(Plato))]
    [Column("plato_id")]
    public int platoId { get; set; }

    public PedidoModel Pedido { get; set; } = null!;

    public PlatoModel Plato { get; set; } = null!;
}