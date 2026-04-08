namespace application.Dtos;

public record PedidoDetalleVM(
    PedidoDto Pedido,
    List<DetallePedidoDto> Detalles,
    string UsuarioNombre
);
