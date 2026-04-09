using application.Dtos;
using application.Enums;
using application.Services;
using Microsoft.AspNetCore.Mvc;

namespace application.Controllers
{
    public class MesaController : Controller
    {
        private readonly IMesaService mesaService;
        public MesaController(IMesaService mesaService)
        {
            this.mesaService = mesaService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int? codigo = null, PisoMesaEnum? piso = null, EstadoMesaEnum? estado = null)
        {
            var filtrarMesaDto = new FiltrarMesaDto(codigo, piso, estado, page);
            var model = await mesaService.obtenerMesaVM(page, filtrarMesaDto);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CrearMesaDto crearMesaDto)
        {
            await mesaService.crearMesaDto(crearMesaDto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var mesa = await mesaService.obtenerMesaPorId(id);
            return View(mesa);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MesaDto mesaDto)
        {
            await mesaService.actualizarMesaDto(mesaDto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await mesaService.eliminarMesa(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
