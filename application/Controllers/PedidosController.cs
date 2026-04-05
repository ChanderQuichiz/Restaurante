using Microsoft.AspNetCore.Mvc;
using application.Models;

namespace application.Controllers
{
    public class PedidosController : Controller
    {
        private static readonly List<PedidoModel> ListaPedidos = new()
        {
            new PedidoModel { id = 1, fecha = DateTime.Today, total = 120.50, estado = "Pendiente", usuarioId = 1, mesaId = 1 },
            new PedidoModel { id = 2, fecha = DateTime.Today.AddDays(-1), total = 300.00, estado = "En proceso", usuarioId = 2, mesaId = 2 }
        };

        public IActionResult Index(string buscar)
        {
            var pedidos = ListaPedidos.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                pedidos = pedidos.Where(p =>
                    p.estado.Contains(buscar, StringComparison.OrdinalIgnoreCase) ||
                    p.id.ToString().Contains(buscar, StringComparison.OrdinalIgnoreCase));
            }

            return View(pedidos.OrderByDescending(p => p.id).ToList());
        }

        public IActionResult Create()
        {
            return View(new PedidoModel { fecha = DateTime.Today, estado = "Pendiente", usuarioId = 1, mesaId = 1, total = 0.01 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PedidoModel pedido)
        {
            if (!ModelState.IsValid)
            {
                return View(pedido);
            }

            pedido.id = ListaPedidos.Any() ? ListaPedidos.Max(p => p.id) + 1 : 1;
            ListaPedidos.Add(pedido);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var pedido = ListaPedidos.FirstOrDefault(p => p.id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PedidoModel pedido)
        {
            if (!ModelState.IsValid)
            {
                return View(pedido);
            }

            var pedidoExistente = ListaPedidos.FirstOrDefault(p => p.id == pedido.id);
            if (pedidoExistente == null)
            {
                return NotFound();
            }

            pedidoExistente.fecha = pedido.fecha;
            pedidoExistente.total = pedido.total;
            pedidoExistente.estado = pedido.estado;
            pedidoExistente.usuarioId = pedido.usuarioId;
            pedidoExistente.mesaId = pedido.mesaId;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var pedido = ListaPedidos.FirstOrDefault(p => p.id == id);
            if (pedido != null)
            {
                ListaPedidos.Remove(pedido);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
