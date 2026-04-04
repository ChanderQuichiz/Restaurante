using System;
using application.Data;
using application.Dtos;
using application.Models;
using application.Utils;

namespace application.Services;

public class PlatoService : IPlatoService
{
    private readonly DbAppContext context;
    public PlatoService(DbAppContext context)
    {
        this.context = context;
    }
    public PlatoDto? actualizarPlatoDto(PlatoDto actualizarPlatoDto)
    {
        PlatoModel? platoencontrado = context.Platos.FirstOrDefault(p => p.id == actualizarPlatoDto.id);
        if(platoencontrado == null)
        {
            return null;
        }
        platoencontrado.nombre = actualizarPlatoDto.nombre;
        platoencontrado.precio = actualizarPlatoDto.precio;
        platoencontrado.categoria = actualizarPlatoDto.categoria;
        platoencontrado.estado = actualizarPlatoDto.estado;
        context.Platos.Update(platoencontrado);
        context.SaveChangesAsync();
        return PlatoMapper.ToPlatoDto(platoencontrado);

    }

    public PlatoDto? crearPlatoDto(CrearPlatoDto crearPlatoDto)
    {
        PlatoModel platoModel = PlatoMapper.ToPlatoModel(crearPlatoDto);
        context.Platos.AddAsync(platoModel);
        context.SaveChangesAsync();
        return PlatoMapper.ToPlatoDto(platoModel);
    }
}
