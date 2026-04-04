using System.ComponentModel.DataAnnotations;

namespace PedidosPagosMVC.Models
{
    public class Pago
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID del pedido es obligatorio")]
        public int PedidoId { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio")]
        [Range(0.01, 9999999, ErrorMessage = "Ingrese un monto válido")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "El método es obligatorio")]
        public string Metodo { get; set; } = "Efectivo";
    }
}
