using System.ComponentModel.DataAnnotations;

namespace application.Dtos;

public record UserDto(
    [Required] int id,
    [Required] string nombre,
    [Required, EmailAddress] string email,
    [Required] string contrasena,
    [Required] string rol,
    [Required] string estado,
    [Required] DateTime fechaExpiracion,
    string? dni
);
