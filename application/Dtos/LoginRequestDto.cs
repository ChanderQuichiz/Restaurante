using System.ComponentModel.DataAnnotations;

namespace application.Dtos;

public class LoginRequestDto
{
    public string tab { get; set; } = "admin";

    [EmailAddress]
    public string? email { get; set; }

    public string? password { get; set; }

    public string? secretKey { get; set; }
}
