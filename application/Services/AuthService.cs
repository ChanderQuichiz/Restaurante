using application.Dtos;
using application.Repositories;

namespace application.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration     _configuration;

    public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _configuration     = configuration;
    }

    public Task<AuthSessionDto?> autenticarAdminConClave(string secretKey)
    {
        var claves = _configuration.GetSection("Auth:SecretKeys")
            .GetChildren()
            .Select(c => c.Value)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Select(c => c!.Trim())
            .ToHashSet(StringComparer.Ordinal);

        if (!claves.Contains(secretKey.Trim()))
            return Task.FromResult<AuthSessionDto?>(null);

        return Task.FromResult<AuthSessionDto?>(new AuthSessionDto("Admin"));
    }

    public async Task<AuthSessionDto?> autenticarEmpleado(string email, string password)
    {
        var user = await _usuarioRepository.GetByEmailAndPassword(email, password);
        if (user == null) return null;

        // Regla de negocio: Admin no puede autenticarse por esta vía
        if (string.Equals(user.rol, "Admin", StringComparison.OrdinalIgnoreCase))
            return null;

        return new AuthSessionDto(user.rol, user.nombre);
    }
}
