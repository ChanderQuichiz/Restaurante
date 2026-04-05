using Microsoft.AspNetCore.Mvc;
using application.Models;

namespace application.Controllers
{
    public class PagosController : Controller
    {
        private static readonly List<PagoModel> ListaPagos = new()
        {
            new PagoModel { id = 1, pedidoId = 1, monto = 120.50, fecha = DateTime.Today, metodoPago = "Efectivo", usuarioId = 1 }
        };

        public IActionResult Index(string buscar)
        {
            var pagos = ListaPagos.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(buscar) && int.TryParse(buscar, out int pedidoId))
            {
                pagos = pagos.Where(p => p.pedidoId == pedidoId);
            }

            return View(pagos.OrderByDescending(p => p.id).ToList());
        }

        public IActionResult Create()
        {
            return View(new PagoModel { fecha = DateTime.Today, metodoPago = "Efectivo", usuarioId = 1, pedidoId = 1, monto = 0.01 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PagoModel pago)
        {
            if (!ModelState.IsValid)
            {
                return View(pago);
            }

            pago.id = ListaPagos.Any() ? ListaPagos.Max(p => p.id) + 1 : 1;
            ListaPagos.Add(pago);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var pago = ListaPagos.FirstOrDefault(p => p.id == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PagoModel pago)
        {
            if (!ModelState.IsValid)
            {
                return View(pago);
            }

            var pagoExistente = ListaPagos.FirstOrDefault(p => p.id == pago.id);
            if (pagoExistente == null)
            {
                return NotFound();
            }

            pagoExistente.pedidoId = pago.pedidoId;
            pagoExistente.monto = pago.monto;
            pagoExistente.fecha = pago.fecha;
            pagoExistente.metodoPago = pago.metodoPago;
            pagoExistente.usuarioId = pago.usuarioId;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var pago = ListaPagos.FirstOrDefault(p => p.id == id);
            if (pago != null)
            {
                ListaPagos.Remove(pago);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
