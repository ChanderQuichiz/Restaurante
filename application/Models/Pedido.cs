using System.ComponentModel.DataAnnotations;

namespace PedidosPagosMVC.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El cliente es obligatorio")]
        public string Cliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio")]
        [Range(0.01, 9999999, ErrorMessage = "Ingrese un monto válido")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        public string Estado { get; set; } = "Pendiente";
    }
}
