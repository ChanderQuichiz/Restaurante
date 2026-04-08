using Microsoft.AspNetCore.Mvc;
using application.Dtos;
using application.Models;
using application.Services;

namespace application.Controllers;

public class PedidosController : Controller
{
    private readonly IPedidoService pedidoService;

    public PedidosController(IPedidoService pedidoService)
    {
        this.pedidoService = pedidoService;
    }

    public async Task<IActionResult> Index(string buscar, string estado, DateTime? fecha)
    {
        List<PedidoDto> lista = await pedidoService.obtenerPedidos(buscar, estado, fecha);

        return View(lista);
    }

    [HttpGet]
    public async Task<IActionResult> Detalle(int id)
    {
        PedidoDetalleVM? modelo = await pedidoService.obtenerDetallePedido(id);
        if (modelo == null)
        {
            return NotFound();
        }
        return View(modelo);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await CargarCombos();

        return View(new CrearPedidoDto(0, string.Empty, 0, "Pendiente", new List<int>(), new List<int>(), new List<double>()));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CrearPedidoDto crearPedidoDto)
    {
        if (!ModelState.IsValid)
        {
            await CargarCombos();
            return View(crearPedidoDto);
        }

        PedidoDto resultado = await pedidoService.crearPedido(crearPedidoDto);

        return RedirectToAction("Index");
    }

    private async Task CargarCombos()
    {
        ViewBag.Meseros = await pedidoService.obtenerMeserosActivos();
        ViewBag.Mesas = await pedidoService.obtenerMesasDisponibles();
        ViewBag.Platos = await pedidoService.obtenerPlatosActivos();
    }
}