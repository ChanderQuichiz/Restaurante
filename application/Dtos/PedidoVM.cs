using application.Enums;

namespace application.Dtos;

public record PedidoVM(
    List<PedidoDto> pedidos,
    int page,
    int totalPages,
    string? buscar,
    EstadoPedidoEnum? estado,
    DateTime? fecha
);