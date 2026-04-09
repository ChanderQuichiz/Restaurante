using application.Enums;

namespace application.Dtos;

public record class FiltrarMesaDto(
     int? codigo,
     PisoMesaEnum? piso,
    EstadoMesaEnum? estado,
    int page = 1
);