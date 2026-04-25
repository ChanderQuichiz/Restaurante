using application.Dtos;
using application.Models;
using application.Repositories;
using application.Utils;

namespace application.Services;

public class PagoService : IPagoService
{
    private readonly IPagoRepository    _pagoRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPedidoRepository  _pedidoRepository;
    private const int PageSize = 10;

    public PagoService(
        IPagoRepository    pagoRepository,
        IUsuarioRepository usuarioRepository,
        IPedidoRepository  pedidoRepository)
    {
        _pagoRepository    = pagoRepository;
        _usuarioRepository = usuarioRepository;
        _pedidoRepository  = pedidoRepository;
    }

    public async Task<PagoDto?> crearPagoDto(CrearPagoDto crearPagoDto)
    {
        var pago       = PagoMapper.ToPagoModel(crearPagoDto);
        pago.fecha     = pago.fecha == default ? DateTime.Now : pago.fecha;

        pago = await _pagoRepository.Add(pago); // correlativo generado en el repo
        await _pagoRepository.LoadUsuario(pago);

        return PagoMapper.ToPagoDto(pago);
    }

    public async Task<List<PagoDto>> obtenerPagos(int page = 1, FiltrarPagoDto? filtro = null)
    {
        page = page < 1 ? 1 : page;
        var lista = await _pagoRepository.GetAll(page, PageSize, filtro);
        return lista.Select(PagoMapper.ToPagoDto).ToList();
    }

    public async Task<int> contarPagos(FiltrarPagoDto? filtro = null)
        => await _pagoRepository.Count(filtro);

    public async Task<PagoVM> obtenerPagoVM(int page = 1, FiltrarPagoDto? filtro = null)
    {
        page = page < 1 ? 1 : page;

        var pagos      = await obtenerPagos(page, filtro);
        var total      = await contarPagos(filtro);
        var totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)PageSize));

        return new PagoVM(pagos, page, totalPages, filtro ?? new FiltrarPagoDto(null, null, null, page));
    }

    public async Task<List<UsuarioModel>> obtenerCajerosActivos()
    {
        var cajeros = await _usuarioRepository.GetActivosByRol("Cajero");
        return cajeros.Count > 0 ? cajeros : await _usuarioRepository.GetActivos();
    }

    public async Task<List<PedidoModel>> obtenerPedidos()
        => await _pedidoRepository.GetTodos();
}
