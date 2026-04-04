using Microsoft.AspNetCore.Mvc;

namespace application.Controllers
{
    public class MesaController : Controller
    {
        // GET: MesaController
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // GET: MesaController/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: MesaController/Create
        [HttpPost]
        public IActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Agregar lógica de guardar
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MesaController/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: MesaController/Edit/5
        [HttpPost]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Agregar lógica de actualizar
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: MesaController/Delete/5
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                // TODO: Agregar lógica de eliminar
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
