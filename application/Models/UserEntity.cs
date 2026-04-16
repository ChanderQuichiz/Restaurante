namespace application.Models;

public class UserEntity
{
    public int id { get; set; }
    public string nombre { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string contrasena { get; set; } = string.Empty;
    public string rol { get; set; } = string.Empty;
    public string estado { get; set; } = string.Empty;
    public DateTime fechaExpiracion { get; set; }
    public string? dni { get; set; }
}
