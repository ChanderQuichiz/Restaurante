using application.Dtos;
using application.Services;
using Microsoft.AspNetCore.Mvc;

namespace application.Controllers;

[Route("empleados")]
public class UserController : Controller
{
    private readonly IUserService userService;

    public UserController(IUserService userService)
    {
        this.userService = userService;
    }

    private bool EsAdmin()
    {
        var authSessionJson = HttpContext.Session.GetString("AuthSession");

        if (string.IsNullOrWhiteSpace(authSessionJson))
        {
            return false;
        }

        try
        {
            var authSession = System.Text.Json.JsonSerializer.Deserialize<AuthSessionDto>(authSessionJson);
            return authSession != null && string.Equals(authSession.rol, "Admin", StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(int page = 1, string? buscar = null, string? rol = null, string? estado = null)
    {
        if (!EsAdmin())
        {
            return RedirectToAction("Index", "Mesa");
        }

        var filtro = new FilterUserDto(buscar, rol, estado, page);
        var model = await userService.obtenerUserVM(page, filtro);
        return View(model);
    }

    [HttpGet("crear")]
    public IActionResult Create()
    {
        if (!EsAdmin())
        {
            return RedirectToAction("Index", "Mesa");
        }

        return View();
    }

    [HttpPost("crear")]
    public async Task<IActionResult> Create(CreateUserDto createUserDto)
    {
        if (!EsAdmin())
        {
            return RedirectToAction("Index", "Mesa");
        }

        if (!ModelState.IsValid)
        {
            return View(createUserDto);
        }

        await userService.crearUser(createUserDto);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        if (!EsAdmin())
        {
            return RedirectToAction("Index", "Mesa");
        }

        var user = await userService.obtenerUserPorId(id);
        if (user == null)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(user);
    }

    [HttpPost("editar")]
    public async Task<IActionResult> Edit(UserDto userDto)
    {
        if (!EsAdmin())
        {
            return RedirectToAction("Index", "Mesa");
        }

        if (!ModelState.IsValid)
        {
            return View(userDto);
        }

        await userService.actualizarUser(userDto);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("eliminar/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!EsAdmin())
        {
            return RedirectToAction("Index", "Mesa");
        }

        await userService.eliminarUser(id);
        return RedirectToAction(nameof(Index));
    }
}
