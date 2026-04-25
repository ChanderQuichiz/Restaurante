using application.Dtos;
using application.Enums;
using application.Models;
using application.Repositories;
using application.Utils;

namespace application.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository  _pedidoRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IMesaRepository    _mesaRepository;
    private readonly IPlatoRepository   _platoRepository;
    private const int PageSize = 10;

    public PedidoService(
        IPedidoRepository  pedidoRepository,
        IUsuarioRepository usuarioRepository,
        IMesaRepository    mesaRepository,
        IPlatoRepository   platoRepository)
    {
        _pedidoRepository  = pedidoRepository;
        _usuarioRepository = usuarioRepository;
        _mesaRepository    = mesaRepository;
        _platoRepository   = platoRepository;
    }

    public async Task<PedidoDto> crearPedido(CrearPedidoDto dto)
    {
        PedidoModel pedido = PedidoMapper.ToPedidoModel(dto);
        pedido.fecha  = DateTime.Now;
        pedido.estado = MapearEstado(dto.estado);
        pedido.total  = dto.platoIds
            .Select((_, i) => dto.cantidades[i] * dto.precios[i])
            .Sum();

        pedido = await _pedidoRepository.Add(pedido); // correlativo generado en el repo

        for (int i = 0; i < dto.platoIds.Count; i++)
        {
            await _pedidoRepository.AddDetalle(new DetallePedidoModel
            {
                pedidoId       = pedido.id,
                platoId        = dto.platoIds[i],
                cantidad       = dto.cantidades[i],
                precioUnitario = dto.precios[i]
            });
        }

        await _pedidoRepository.SaveChanges();

        // Enriquecer con mesero para la respuesta
        UsuarioModel? mesero = await _usuarioRepository.GetById(pedido.meseroId);
        if (mesero != null) pedido.Mesero = mesero;

        return PedidoMapper.ToPedidoDto(pedido);
    }

    public async Task<PedidoVM> obtenerPedidosVM(
        int page = 1, string? buscar = null,
        EstadoPedidoEnum? estado = null, DateTime? fecha = null)
    {
        page = page < 1 ? 1 : page;

        var lista      = await _pedidoRepository.GetAll(page, PageSize, buscar, estado, fecha);
        var total      = await _pedidoRepository.Count(buscar, estado, fecha);
        var totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)PageSize));

        return new PedidoVM(
            lista.Select(PedidoMapper.ToPedidoDto).ToList(),
            page, totalPages, buscar, estado, fecha);
    }

    public async Task<PedidoDetalleVM?> obtenerDetallePedido(int pedidoId)
    {
        PedidoModel? pedido = await _pedidoRepository.GetByIdConDetalle(pedidoId);
        if (pedido == null) return null;

        var detalles = pedido.Detalles.Select(PedidoMapper.ToDetallePedidoDto).ToList();
        return new PedidoDetalleVM(PedidoMapper.ToPedidoDto(pedido), detalles, pedido.Mesero.nombre);
    }

    public async Task<List<UsuarioModel>> obtenerMeserosActivos()
        => await _usuarioRepository.GetActivosByRol("Mesero");

    public async Task<List<MesaModel>> obtenerMesasDisponibles()
        => await _mesaRepository.GetAll(
            page: 1, pageSize: int.MaxValue,
            filtro: new FiltrarMesaDto(null, null, EstadoMesaEnum.LIBRE, 1));

    public async Task<List<PlatoModel>> obtenerPlatosActivos()
        => await _platoRepository.GetActivos();

    // ── Helpers ────────────────────────────────────────────────────────────────

    private static string MapearEstado(EstadoPedidoEnum estado) => estado switch
    {
        EstadoPedidoEnum.EnProceso => "En proceso",
        EstadoPedidoEnum.Entregado => "Entregado",
        _                         => "Pendiente"
    };
}
