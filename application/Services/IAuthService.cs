using application.Dtos;

namespace application.Services;

public interface IAuthService
{
    Task<AuthSessionDto?> autenticarAdminConClave(string secretKey);
    Task<AuthSessionDto?> autenticarEmpleado(string email, string password);
}
