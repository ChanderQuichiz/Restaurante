using System;

namespace application.Dtos;

public record MesaVM(
    List<MesaDto> mesas,
    int page,
    int totalPages
);
