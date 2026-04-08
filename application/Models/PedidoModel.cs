using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace application.Models;

[Table("pedidos")]
public class PedidoModel
{
    [Key]
    public int id { get; set; }

    [Column("usuario_id")]
    public int meseroId { get; set; }

    [Column("dni_cliente")]
    public string dniCliente { get; set; } = string.Empty;

    [Column("mesa_id")]
    public int mesaId { get; set; }

    [Column("fecha")]
    public DateTime fecha { get; set; }

    [Column("total")]
    public double total { get; set; }

    [Column("estado")]
    public string estado { get; set; } = string.Empty;

    [ForeignKey(nameof(meseroId))]
    public UsuarioModel Mesero { get; set; } = null!;

    [ForeignKey(nameof(mesaId))]
    public MesaModel Mesa { get; set; } = null!;

    public ICollection<DetallePedidoModel> Detalles { get; set; } = new List<DetallePedidoModel>();
}