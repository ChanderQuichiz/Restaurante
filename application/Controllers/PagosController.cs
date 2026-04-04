using Microsoft.AspNetCore.Mvc;
using PedidosPagosMVC.Models;

namespace PedidosPagosMVC.Controllers
{
    public class PagosController : Controller
    {
        private static readonly List<Pago> ListaPagos = new()
        {
            new Pago { Id = 1, PedidoId = 1, Monto = 120.50m, Fecha = DateTime.Today, Metodo = "Efectivo" }
        };

        public IActionResult Index(string buscar)
        {
            var pagos = ListaPagos.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(buscar) && int.TryParse(buscar, out int pedidoId))
            {
                pagos = pagos.Where(p => p.PedidoId == pedidoId);
            }

            return View(pagos.OrderByDescending(p => p.Id).ToList());
        }

        public IActionResult Create()
        {
            return View(new Pago { Fecha = DateTime.Today, Metodo = "Efectivo" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pago pago)
        {
            if (!ModelState.IsValid)
            {
                return View(pago);
            }

            pago.Id = ListaPagos.Any() ? ListaPagos.Max(p => p.Id) + 1 : 1;
            ListaPagos.Add(pago);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var pago = ListaPagos.FirstOrDefault(p => p.Id == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Pago pago)
        {
            if (!ModelState.IsValid)
            {
                return View(pago);
            }

            var pagoExistente = ListaPagos.FirstOrDefault(p => p.Id == pago.Id);
            if (pagoExistente == null)
            {
                return NotFound();
            }

            pagoExistente.PedidoId = pago.PedidoId;
            pagoExistente.Monto = pago.Monto;
            pagoExistente.Fecha = pago.Fecha;
            pagoExistente.Metodo = pago.Metodo;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var pago = ListaPagos.FirstOrDefault(p => p.Id == id);
            if (pago != null)
            {
                ListaPagos.Remove(pago);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
