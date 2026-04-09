using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using application.Data;
using application.Models;

namespace application.Controllers
{
    public class PagosController : Controller
    {
        private readonly DbAppContext _context;

        public PagosController(DbAppContext context)
        {
            _context = context;
        }

        public IActionResult Index(string buscar, string metodo, DateTime? fecha)
        {
            var pagos = _context.Pagos
                .Include(p => p.Pedido)
                .Include(p => p.Usuario)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                var txt = buscar.Trim();
                pagos = pagos.Where(p => p.pedidoId.ToString().Contains(txt));
            }

            if (!string.IsNullOrWhiteSpace(metodo))
            {
                pagos = pagos.Where(p => p.metodoPago == metodo);
            }

            if (fecha.HasValue)
            {
                var f = fecha.Value.Date;
                pagos = pagos.Where(p => p.fecha.Date == f);
            }

            return View(pagos
                .OrderByDescending(p => p.id)
                .ToList());
        }

        public IActionResult Create(int pedidoId)
        {
            CargarCajeros();
            CargarPedidos();

            var pago = new PagoModel
            {
                pedidoId = pedidoId,
                fecha = DateTime.Now
            };

            return View(pago);
        }

        [HttpPost]
        public IActionResult Create(PagoModel pago)
        {
            if (!ModelState.IsValid)
            {
                CargarCajeros();
                CargarPedidos();
                return View(pago);
            }

            if (pago.fecha == default)
            {
                pago.fecha = DateTime.Now;
            }

            _context.Pagos.Add(pago);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private void CargarCajeros()
        {
            List<UsuarioModel> cajeros = _context.Usuarios
                .Where(u => u.estado == "Activo" && u.rol == "Cajero")
                .OrderBy(u => u.nombre)
                .ToList();

            if (cajeros.Count == 0)
            {
                cajeros = _context.Usuarios
                    .Where(u => u.estado == "Activo")
                    .OrderBy(u => u.nombre)
                    .ToList();
            }

            ViewBag.Cajeros = cajeros;
        }

        private void CargarPedidos()
        {
            List<PedidoModel> pedidos = _context.Pedidos
                .OrderByDescending(p => p.id)
                .ToList();

            ViewBag.Pedidos = pedidos;
        }
    }
}