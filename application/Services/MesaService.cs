using application.Dtos;
using application.Enums;
using application.Models;
using application.Repositories;
using application.Utils;

namespace application.Services;

public class MesaService : IMesaService
{
    private readonly IMesaRepository _mesaRepository;
    private const int PageSize = 10;

    public MesaService(IMesaRepository mesaRepository)
    {
        _mesaRepository = mesaRepository;
    }

    public async Task<MesaDto?> crearMesaDto(CrearMesaDto crearMesaDto)
    {
        MesaModel model = MesaMapper.ToMesaModel(crearMesaDto);
        model = await _mesaRepository.Add(model); // correlativo generado en el repo
        return MesaMapper.ToMesaDto(model);
    }

    public async Task<MesaDto?> actualizarMesaDto(MesaDto actualizarMesaDto)
    {
        MesaModel? mesa = await _mesaRepository.GetById(actualizarMesaDto.id);
        if (mesa == null) return null;

        mesa.numeroPiso = actualizarMesaDto.numeroPiso;
        mesa.capacidad  = actualizarMesaDto.capacidad;
        mesa.estado     = actualizarMesaDto.estado.ToString();

        await _mesaRepository.Update();
        return MesaMapper.ToMesaDto(mesa);
    }

    public async Task<MesaDto?> obtenerMesaPorId(int id)
    {
        MesaModel? mesa = await _mesaRepository.GetById(id);
        return mesa != null ? MesaMapper.ToMesaDto(mesa) : null;
    }

    public async Task<bool> eliminarMesa(int id)
    {
        MesaModel? mesa = await _mesaRepository.GetById(id);
        if (mesa == null) return false;

        mesa.estado = EstadoMesaEnum.INACTIVO.ToString();
        await _mesaRepository.Update();
        return true;
    }

    public async Task<List<MesaDto>> obtenerMesas(int page = 1, FiltrarMesaDto? filtro = null)
    {
        page = page < 1 ? 1 : page;
        var mesas = await _mesaRepository.GetAll(page, PageSize, filtro);
        return mesas.Select(MesaMapper.ToMesaDto).ToList();
    }

    public async Task<int> contarMesas(FiltrarMesaDto? filtro = null)
        => await _mesaRepository.Count(filtro);

    public async Task<MesaVM> obtenerMesaVM(int page = 1, FiltrarMesaDto? filtro = null)
    {
        page = page < 1 ? 1 : page;

        var mesas      = await obtenerMesas(page, filtro);
        var total      = await contarMesas(filtro);
        var totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)PageSize));

        return new MesaVM(mesas, page, totalPages, filtro ?? new FiltrarMesaDto(null, null, null, page));
    }
}
