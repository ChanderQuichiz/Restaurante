using application.Enums;

namespace application.Dtos;

public record class FiltrarMesaDto(
    string? codigo,
     PisoMesaEnum? piso,
    EstadoMesaEnum? estado,
    int page = 1
);