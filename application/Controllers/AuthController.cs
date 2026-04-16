using System.Text.Json;
using application.Dtos;
using application.Services;
using Microsoft.AspNetCore.Mvc;

namespace application.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    private readonly IAuthService authService;

    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        var sesion = HttpContext.Session.GetString("AuthSession");
        if (!string.IsNullOrWhiteSpace(sesion))
        {
            return RedirectToAction("Index", "Mesa");
        }

        return View(new LoginRequestDto());
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        if (request.tab == "empleado")
        {
            if (string.IsNullOrWhiteSpace(request.email) || string.IsNullOrWhiteSpace(request.password))
            {
                ModelState.AddModelError(string.Empty, "Email y contraseña son obligatorios para empleado.");
                return View(request);
            }

            var empleado = await authService.autenticarEmpleado(request.email.Trim(), request.password.Trim());
            if (empleado == null)
            {
                ModelState.AddModelError(string.Empty, 
                "Credenciales inválidas para empleado.");
                return View(request);
            }

            var authSession = new AuthSessionDto(empleado.rol, empleado.nombre);
            HttpContext.Session.SetString("AuthSession", JsonSerializer.Serialize(authSession));
            return RedirectToAction("Index", "Mesa");
        }

        if (string.IsNullOrWhiteSpace(request.secretKey))
        {
            ModelState.AddModelError(string.Empty, "La clave secreta de administrador es obligatoria.");
            return View(request);
        }

        var admin = await authService.autenticarAdminConClave(request.secretKey.Trim());
        if (admin == null)
        {
            ModelState.AddModelError(string.Empty, "Clave secreta incorrecta.");
            return View(request);
        }

        var adminSession = new AuthSessionDto(admin.rol);
        HttpContext.Session.SetString("AuthSession", JsonSerializer.Serialize(adminSession));
        return RedirectToAction("Index", "Mesa");
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }
}
