using application.Data;
using application.Dtos;
using application.Models;
using Microsoft.EntityFrameworkCore;

namespace application.Services;

public class AuthService : IAuthService
{
    private readonly DbAppContext context;
    private readonly IConfiguration configuration;

    public AuthService(DbAppContext context, IConfiguration configuration)
    {
        this.context = context;
        this.configuration = configuration;
    }

    public Task<AuthSessionDto?> autenticarAdminConClave(string secretKey)
    {
        var claves = configuration.GetSection("Auth:SecretKeys")
            .GetChildren()
            .Select(clave => clave.Value)
            .Where(clave => !string.IsNullOrWhiteSpace(clave))
            .Select(clave => clave!.Trim())
            .ToHashSet(StringComparer.Ordinal);

        if (!claves.Contains(secretKey.Trim()))
        {
            return Task.FromResult<AuthSessionDto?>(null);
        }

        return Task.FromResult<AuthSessionDto?>(new AuthSessionDto("Admin"));
    }

    public async Task<AuthSessionDto?> autenticarEmpleado(string email, string password)
    {
        var user = await context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.email == email && u.contrasena == password && u.estado == "Activo");

        if (user == null)
        {
            return null;
        }

        if (string.Equals(user.rol, "Admin", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return new AuthSessionDto(user.rol, user.nombre);
    }
}
