using System;
using application.Data;
using application.Dtos;
using application.Models;
using application.Utils;
using Microsoft.EntityFrameworkCore;

namespace application.Services;

public class MesaService : IMesaService
{
    private readonly DbAppContext context;
    private const int PageSize = 10;

    public MesaService(DbAppContext context)
    {
        this.context = context;
    }

    public async Task<MesaDto?> actualizarMesaDto(MesaDto actualizarMesaDto)
    {

        MesaModel? mesa = await context.Mesas.FirstOrDefaultAsync(m => m.id == actualizarMesaDto.id);
        if(mesa == null)
        {
            return null;
        }
        mesa.numeroPiso = actualizarMesaDto.numeroPiso;
        mesa.capacidad = actualizarMesaDto.capacidad;
        mesa.estado = actualizarMesaDto.estado;
        await context.SaveChangesAsync();
        return MesaMapper.ToMesaDto(mesa);
    }

    public async Task<MesaDto?> crearMesaDto(CrearMesaDto crearMesaDto)
    {

       MesaModel model = MesaMapper.ToMesaModel(crearMesaDto);
        await context.Mesas.AddAsync(model);
       await context.SaveChangesAsync();
        return MesaMapper.ToMesaDto(model);
    }

    public async Task<MesaDto?> obtenerMesaPorId(int id)
    {
        MesaModel? mesa = await context.Mesas.FirstOrDefaultAsync(m => m.id == id);
        return mesa != null ? MesaMapper.ToMesaDto(mesa) : null;
    }
    public async Task<bool> eliminarMesa(int id)
    {
        MesaModel? mesa = await context.Mesas.FirstOrDefaultAsync(m => m.id == id);
        if (mesa == null)
        {
            return false;
        }
        mesa.estado = "INACTIVO";
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<List<MesaDto>> obtenerMesas(int page = 1)

    {

        if (page < 1)
        {
            page = 1;
        }
        return await context.Mesas.Select(m => MesaMapper
        .ToMesaDto(m))
        .AsNoTracking()
        .Skip((page - 1) * PageSize)
        .Take(PageSize)
        .ToListAsync();
    }

    public async Task<int> contarMesas()
    {
        return await context.Mesas.CountAsync();
    }

    public async Task<MesaVM> obtenerMesaVM(int page = 1)
    {
        if (page < 1)
        {
            page = 1;
        }

        var mesas = await obtenerMesas(page);
        var totalMesas = await contarMesas();
        var totalPages = (int)Math.Ceiling(totalMesas / (double)PageSize);

        if (totalPages < 1)
        {
            totalPages = 1;
        }

        return new MesaVM(mesas, page, totalPages);
    }
}
