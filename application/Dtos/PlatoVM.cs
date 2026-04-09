using System;

namespace application.Dtos;

public record PlatoVM(
    List<PlatoDto> platos,
    int page,
    int totalPages,
    FiltrarPlatoDto filtros
);
