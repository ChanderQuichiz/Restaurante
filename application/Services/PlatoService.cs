using System;
using application.Data;
using application.Dtos;
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
        platoencontrado.categoria = actualizarPlatoDto.categoria;
        platoencontrado.estado = actualizarPlatoDto.estado;
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
        plato.estado = "INACTIVO";
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<PlatoDto?> obtenerPlatoPorId(int id)
    {
        PlatoModel? plato = await context.Platos.FirstOrDefaultAsync(p => p.id == id);
        return plato != null ? PlatoMapper.ToPlatoDto(plato) : null;
    }

    public async Task<List<PlatoDto>> obtenerPlatos(int page = 1)
    {
        if (page < 1)
        {
            page = 1;
        }
        return await context.Platos.Select(p => PlatoMapper
        .ToPlatoDto(p))
        .AsNoTracking()
        .Skip((page - 1) * PageSize)
        .Take(PageSize)
        .ToListAsync();
    }

    public async Task<int> contarPlatos()
    {
        return await context.Platos.CountAsync();
    }

    public async Task<PlatoVM> obtenerPlatoVM(int page = 1)
    {
        if (page < 1)
        {
            page = 1;
        }

        var platos = await obtenerPlatos(page);
        var totalPlatos = await contarPlatos();
        var totalPages = (int)Math.Ceiling(totalPlatos / (double)PageSize);

        if (totalPages < 1)
        {
            totalPages = 1;
        }

        return new PlatoVM(platos, page, totalPages);
    }
}
