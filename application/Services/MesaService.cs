using System;
using application.Data;
using application.Dtos;
using application.Enums;
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
    MesaModel? mesa = await context.Mesas
        .FirstOrDefaultAsync(m => m.id == actualizarMesaDto.id);

    if (mesa == null)
        return null;

    mesa.numeroPiso = actualizarMesaDto.numeroPiso;
    mesa.capacidad = actualizarMesaDto.capacidad;
    mesa.estado = actualizarMesaDto.estado.ToString();

    await context.SaveChangesAsync();

    return MesaMapper.ToMesaDto(mesa);
}

public async Task<MesaDto?> crearMesaDto(CrearMesaDto crearMesaDto)
{
    MesaModel model = MesaMapper.ToMesaModel(crearMesaDto);
    model.correlativo = await obtenerSiguienteCorrelativoAsync();

    await context.Mesas.AddAsync(model);
    await context.SaveChangesAsync();

    return MesaMapper.ToMesaDto(model);
}

public async Task<MesaDto?> obtenerMesaPorId(int id)
{
    MesaModel? mesa = await context.Mesas
        .FirstOrDefaultAsync(m => m.id == id);

    return mesa != null ? MesaMapper.ToMesaDto(mesa) : null;
}

public async Task<bool> eliminarMesa(int id)
{
    MesaModel? mesa = await context.Mesas
        .FirstOrDefaultAsync(m => m.id == id);

    if (mesa == null)
        return false;

    mesa.estado = EstadoMesaEnum.INACTIVO.ToString();
    await context.SaveChangesAsync();

    return true;
}

public async Task<List<MesaDto>> obtenerMesas(int page = 1, FiltrarMesaDto? filtro = null)
{
    page = page < 1 ? 1 : page;

    var query = context.Mesas.AsQueryable();

    if (filtro != null)
    {
        if (!string.IsNullOrWhiteSpace(filtro.codigo))
        {
            var codigo = filtro.codigo.Trim();
            query = query.Where(m => m.correlativo.Contains(codigo));
        }

        if (filtro.piso != null)
            query = query.Where(m => m.numeroPiso == (int)filtro.piso.Value);

        if (filtro.estado != null)
            query = query.Where(m => m.estado == filtro.estado.ToString());
    }

    return await query
        .Select(m => MesaMapper.ToMesaDto(m))
        .AsNoTracking()
        .Skip((page - 1) * PageSize)
        .Take(PageSize)
        .ToListAsync();
}

public async Task<int> contarMesas(FiltrarMesaDto? filtro = null)
{
    var query = context.Mesas.AsQueryable();

    if (filtro != null)
    {
        if (!string.IsNullOrWhiteSpace(filtro.codigo))
        {
            var codigo = filtro.codigo.Trim();
            query = query.Where(m => m.correlativo.Contains(codigo));
        }

        if (filtro.piso != null)
            query = query.Where(m => m.numeroPiso == (int)filtro.piso.Value);

        if (filtro.estado != null)
            query = query.Where(m => m.estado == filtro.estado.ToString());
    }

    return await query.CountAsync();
}

public async Task<MesaVM> obtenerMesaVM(int page = 1, FiltrarMesaDto? filtro = null)
{
    page = page < 1 ? 1 : page;

    var mesas = await obtenerMesas(page, filtro);
    var totalMesas = await contarMesas(filtro);

    var totalPages = (int)Math.Ceiling(totalMesas / (double)PageSize);
    totalPages = totalPages == 0 ? 1 : totalPages;

    return new MesaVM(mesas, page, totalPages, filtro ?? new FiltrarMesaDto(null, null, null, page));
}

private async Task<string> obtenerSiguienteCorrelativoAsync()
{
    var ultimoCorrelativo = await context.Mesas
        .AsNoTracking()
        .Where(m => !string.IsNullOrWhiteSpace(m.correlativo))
        .OrderByDescending(m => m.id)
        .Select(m => m.correlativo)
        .FirstOrDefaultAsync();

    if (!string.IsNullOrWhiteSpace(ultimoCorrelativo))
    {
        return construirCorrelativo(ultimoCorrelativo, "MES");
    }

    var totalMesas = await context.Mesas.CountAsync();
    return $"MES-{totalMesas + 1:D2}";
}

private static string construirCorrelativo(string correlativoActual, string prefijo)
{
    var partes = correlativoActual.Split('-', 2);

    if (partes.Length == 2 && int.TryParse(partes[1], out var numero))
    {
        return $"{prefijo}-{numero + 1:D2}";
    }

    return $"{prefijo}-01";
}

  

}
