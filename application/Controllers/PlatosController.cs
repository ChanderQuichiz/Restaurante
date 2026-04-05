using Microsoft.AspNetCore.Mvc;

namespace application.Controllers
{
    public class PlatosController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(IFormCollection collection)
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}