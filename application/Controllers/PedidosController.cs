using Microsoft.AspNetCore.Mvc;
using PedidosPagosMVC.Models;

namespace PedidosPagosMVC.Controllers
{
    public class PedidosController : Controller
    {
        private static readonly List<Pedido> ListaPedidos = new()
        {
            new Pedido { Id = 1, Cliente = "Juan Pérez", Fecha = DateTime.Today, Monto = 120.50m, Estado = "Pendiente" },
            new Pedido { Id = 2, Cliente = "María López", Fecha = DateTime.Today.AddDays(-1), Monto = 300.00m, Estado = "En proceso" }
        };

        public IActionResult Index(string buscar)
        {
            var pedidos = ListaPedidos.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                pedidos = pedidos.Where(p =>
                    p.Cliente.Contains(buscar, StringComparison.OrdinalIgnoreCase) ||
                    p.Estado.Contains(buscar, StringComparison.OrdinalIgnoreCase));
            }

            return View(pedidos.OrderByDescending(p => p.Id).ToList());
        }

        public IActionResult Create()
        {
            return View(new Pedido { Fecha = DateTime.Today, Estado = "Pendiente" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pedido pedido)
        {
            if (!ModelState.IsValid)
            {
                return View(pedido);
            }

            pedido.Id = ListaPedidos.Any() ? ListaPedidos.Max(p => p.Id) + 1 : 1;
            ListaPedidos.Add(pedido);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var pedido = ListaPedidos.FirstOrDefault(p => p.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Pedido pedido)
        {
            if (!ModelState.IsValid)
            {
                return View(pedido);
            }

            var pedidoExistente = ListaPedidos.FirstOrDefault(p => p.Id == pedido.Id);
            if (pedidoExistente == null)
            {
                return NotFound();
            }

            pedidoExistente.Cliente = pedido.Cliente;
            pedidoExistente.Fecha = pedido.Fecha;
            pedidoExistente.Monto = pedido.Monto;
            pedidoExistente.Estado = pedido.Estado;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var pedido = ListaPedidos.FirstOrDefault(p => p.Id == id);
            if (pedido != null)
            {
                ListaPedidos.Remove(pedido);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
