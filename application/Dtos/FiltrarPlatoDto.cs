using application.Enums;

namespace application.Dtos;

public record FiltrarPlatoDto(
    string? buscar,
    CategoriaPlatoEnum? categoria,
    EstadoPlatoEnum? estado,
    int page = 1
);