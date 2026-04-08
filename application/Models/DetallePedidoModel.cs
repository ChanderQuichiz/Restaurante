using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace application.Models;

[Table("detalle_pedido")]
public class DetallePedidoModel
{
    [Key]
    public int id { get; set; }

    [Column("pedido_id")]
    public int pedidoId { get; set; }

    [Column("plato_id")]
    public int platoId { get; set; }

    [Column("cantidad")]
    public int cantidad { get; set; }

    [Column("precio_unitario")]
    public double precioUnitario { get; set; }

    [ForeignKey(nameof(pedidoId))]
    public PedidoModel Pedido { get; set; } = null!;

    [ForeignKey(nameof(platoId))]
    public PlatoModel Plato { get; set; } = null!;
}