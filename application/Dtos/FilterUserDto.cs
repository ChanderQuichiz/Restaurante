namespace application.Dtos;

public record FilterUserDto(
    string? buscar,
    string? rol,
    string? estado,
    int page = 1
);
