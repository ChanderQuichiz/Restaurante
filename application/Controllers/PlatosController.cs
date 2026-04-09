using application.Dtos;
using application.Enums;
using application.Services;
using Microsoft.AspNetCore.Mvc;

namespace application.Controllers
{
    public class PlatosController : Controller
    {
private readonly IPlatoService platoService;

public PlatosController(IPlatoService platoService)
        {
            this.platoService = platoService;
        }


        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string? buscar = null, CategoriaPlatoEnum? categoria = null, EstadoPlatoEnum? estado = null)
        {
            var filtro = new FiltrarPlatoDto(buscar, categoria, estado, page);
            var model = await platoService.obtenerPlatoVM(page, filtro);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {   
            var plato = await platoService.obtenerPlatoPorId(id);
            return View(plato);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CrearPlatoDto crearPlatoDto)
        {
            await platoService.crearPlatoDto(crearPlatoDto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PlatoDto platoDto)
        {
            await platoService.actualizarPlatoDto(platoDto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await platoService.eliminarPlato(id);
            return RedirectToAction(nameof(Index));
        }
    }
}