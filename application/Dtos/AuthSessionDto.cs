namespace application.Dtos;

public record AuthSessionDto(
    string rol,
    string? nombre = null
);
