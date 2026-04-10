namespace application.Dtos;

public record PagoVM(
    List<PagoDto> pagos,
    int page,
    int totalPages,
    FiltrarPagoDto filtros
);