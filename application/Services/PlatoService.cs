using application.Dtos;
using application.Enums;
using application.Models;
using application.Repositories;
using application.Utils;

namespace application.Services;

public class PlatoService : IPlatoService
{
    private readonly IPlatoRepository _platoRepository;
    private const int PageSize = 10;

    public PlatoService(IPlatoRepository platoRepository)
    {
        _platoRepository = platoRepository;
    }

    public async Task<PlatoDto?> crearPlatoDto(CrearPlatoDto crearPlatoDto)
    {
        PlatoModel model = PlatoMapper.ToPlatoModel(crearPlatoDto);
        model = await _platoRepository.Add(model); 
        return PlatoMapper.ToPlatoDto(model);
    }

    public async Task<PlatoDto?> actualizarPlatoDto(PlatoDto actualizarPlatoDto)
    {
        PlatoModel? plato = await _platoRepository.GetById(actualizarPlatoDto.id);
        if (plato == null) return null;

        plato.nombre    = actualizarPlatoDto.nombre;
        plato.precio    = actualizarPlatoDto.precio;
        plato.categoria = actualizarPlatoDto.categoria.ToString();
        plato.estado    = actualizarPlatoDto.estado.ToString();

        await _platoRepository.Update();
        return PlatoMapper.ToPlatoDto(plato);
    }

    public async Task<PlatoDto?> obtenerPlatoPorId(int id)
    {
        PlatoModel? plato = await _platoRepository.GetById(id);
        return plato != null ? PlatoMapper.ToPlatoDto(plato) : null;
    }

    public async Task<bool> eliminarPlato(int id)
    {
        PlatoModel? plato = await _platoRepository.GetById(id);
        if (plato == null) return false;

        plato.estado = EstadoPlatoEnum.INACTIVO.ToString();
        await _platoRepository.Update();
        return true;
    }

    public async Task<List<PlatoDto>> obtenerPlatos(int page = 1, FiltrarPlatoDto? filtro = null)
    {
        page = page < 1 ? 1 : page;
        var platos = await _platoRepository.GetAll(page, PageSize, filtro);
        return platos.Select(PlatoMapper.ToPlatoDto).ToList();
    }

    public async Task<int> contarPlatos(FiltrarPlatoDto? filtro = null)
        => await _platoRepository.Count(filtro);

    public async Task<PlatoVM> obtenerPlatoVM(int page = 1, FiltrarPlatoDto? filtro = null)
    {
        page   = page < 1 ? 1 : page;
        filtro ??= new FiltrarPlatoDto(null, null, null, page);

        var platos     = await obtenerPlatos(page, filtro);
        var total      = await contarPlatos(filtro);
        var totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)PageSize));

        return new PlatoVM(platos, page, totalPages, filtro with { page = page });
    }
}
