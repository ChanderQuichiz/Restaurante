using System;
using application.Data;
using application.Dtos;
using application.Enums;
using application.Models;
using application.Utils;
using Microsoft.EntityFrameworkCore;

namespace application.Services;

public class PlatoService : IPlatoService
{
    private readonly DbAppContext context;
    private const int PageSize = 10;

    public PlatoService(DbAppContext context)
    {
        this.context = context;
    }
    public async Task<PlatoDto?> actualizarPlatoDto(PlatoDto actualizarPlatoDto)
    {
        PlatoModel? platoencontrado = await context.Platos.FirstOrDefaultAsync(p => p.id == actualizarPlatoDto.id);
        if(platoencontrado == null)
        {
            return null;
        }
        platoencontrado.nombre = actualizarPlatoDto.nombre;
        platoencontrado.precio = actualizarPlatoDto.precio;
        platoencontrado.categoria = actualizarPlatoDto.categoria.ToString();
        platoencontrado.estado = actualizarPlatoDto.estado.ToString();
        await context.SaveChangesAsync();
        return PlatoMapper.ToPlatoDto(platoencontrado);

    }

    public async Task<PlatoDto?> crearPlatoDto(CrearPlatoDto crearPlatoDto)
    {
        PlatoModel platoModel = PlatoMapper.ToPlatoModel(crearPlatoDto);
        await context.Platos.AddAsync(platoModel);
        await context.SaveChangesAsync();
        return PlatoMapper.ToPlatoDto(platoModel);
    }

    public async Task<bool> eliminarPlato(int id)
    {
        PlatoModel? plato = await context.Platos.FirstOrDefaultAsync(p => p.id == id);
        if (plato == null)
        {
            return false;
        }
        plato.estado = EstadoPlatoEnum.INACTIVO.ToString();
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<PlatoDto?> obtenerPlatoPorId(int id)
    {
        PlatoModel? plato = await context.Platos.FirstOrDefaultAsync(p => p.id == id);
        return plato != null ? PlatoMapper.ToPlatoDto(plato) : null;
    }

    public async Task<List<PlatoDto>> obtenerPlatos(int page = 1, FiltrarPlatoDto? filtro = null)
    {
        if (page < 1)
        {
            page = 1;
        }

        IQueryable<PlatoModel> query = context.Platos.AsQueryable();

        if (filtro != null)
        {
            if (!string.IsNullOrWhiteSpace(filtro.buscar))
            {
                var buscar = filtro.buscar.Trim();
                query = query.Where(p => p.nombre.Contains(buscar));
            }

            if (filtro.categoria.HasValue)
            {
                query = query.Where(p => p.categoria == filtro.categoria.Value.ToString());
            }

            if (filtro.estado.HasValue)
            {
                query = query.Where(p => p.estado == filtro.estado.Value.ToString());
            }
        }

        return await query
            .Select(p => PlatoMapper.ToPlatoDto(p))
            .AsNoTracking()
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();
    }

    public async Task<int> contarPlatos(FiltrarPlatoDto? filtro = null)
    {
        IQueryable<PlatoModel> query = context.Platos.AsQueryable();

        if (filtro != null)
        {
            if (!string.IsNullOrWhiteSpace(filtro.buscar))
            {
                var buscar = filtro.buscar.Trim();
                query = query.Where(p => p.nombre.Contains(buscar));
            }

            if (filtro.categoria.HasValue)
            {
                query = query.Where(p => p.categoria == filtro.categoria.Value.ToString());
            }

            if (filtro.estado.HasValue)
            {
                query = query.Where(p => p.estado == filtro.estado.Value.ToString());
            }
        }

        return await query.CountAsync();
    }

    public async Task<PlatoVM> obtenerPlatoVM(int page = 1, FiltrarPlatoDto? filtro = null)
    {
        if (page < 1)
        {
            page = 1;
        }

        filtro ??= new FiltrarPlatoDto(null, null, null, page);

        var platos = await obtenerPlatos(page, filtro);
        var totalPlatos = await contarPlatos(filtro);
        var totalPages = (int)Math.Ceiling(totalPlatos / (double)PageSize);

        if (totalPages < 1)
        {
            totalPages = 1;
        }

        return new PlatoVM(platos, page, totalPages, filtro with { page = page });
    }
}
