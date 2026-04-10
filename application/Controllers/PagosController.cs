using application.Dtos;
using application.Services;
using Microsoft.AspNetCore.Mvc;

namespace application.Controllers
{
    public class PagosController : Controller
    {
        private readonly IPagoService pagoService;

        public PagosController(IPagoService pagoService)
        {
            this.pagoService = pagoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string? buscar = null, string? metodo = null, DateTime? fecha = null)
        {
            var filtro = new FiltrarPagoDto(buscar, metodo, fecha, page);
            var model = await pagoService.obtenerPagoVM(page, filtro);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await CargarDatosFormulario();
            return View(new CrearPagoDto(0, 0, 0, DateTime.Today, string.Empty));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CrearPagoDto crearPagoDto)
        {
            if (!ModelState.IsValid)
            {
                await CargarDatosFormulario();
                return View(crearPagoDto);
            }

            await pagoService.crearPagoDto(crearPagoDto);

            return RedirectToAction(nameof(Index));
        }

        private async Task CargarDatosFormulario()
        {
            ViewBag.Cajeros = await pagoService.obtenerCajerosActivos();
            ViewBag.Pedidos = await pagoService.obtenerPedidos();
        }
    }
}