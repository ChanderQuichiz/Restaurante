using application.Dtos;
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
        
        // GET: MesaController
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            var model = await mesaService.obtenerMesaVM(page);
            return View(model);
        }

        // GET: MesaController/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: MesaController/Create
        [HttpPost]
        public async Task<IActionResult> Create(CrearMesaDto crearMesaDto)
        {
            await mesaService.crearMesaDto(crearMesaDto);
            return RedirectToAction(nameof(Index));
        }

        // GET: MesaController/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var mesa = await mesaService.obtenerMesaPorId(id);
            return View(mesa);
        }

        // POST: MesaController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(MesaDto mesaDto)
        {
            await mesaService.actualizarMesaDto(mesaDto);
            return RedirectToAction(nameof(Index));
        }

        // POST: MesaController/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await mesaService.eliminarMesa(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
