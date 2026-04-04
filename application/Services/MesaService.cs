using System;
using application.Data;
using application.Dtos;
using application.Models;
using application.Utils;

namespace application.Services;

public class MesaService : IMesaService
{
    private readonly DbAppContext context;
    public MesaService(DbAppContext context)
    {
        this.context = context;
    }

    public MesaDto? actualizarMesaDto(MesaDto actualizarMesaDto)
    {

        MesaModel? mesa = context.Mesas.FirstOrDefault(m => m.id == actualizarMesaDto.id);
        if(mesa == null)
        {
            return null;
        }
        mesa.numeroPiso = actualizarMesaDto.numeroPiso;
        mesa.capacidad = actualizarMesaDto.capacidad;
        mesa.estado = actualizarMesaDto.estado;
        context.Mesas.Update(mesa);
        context.SaveChangesAsync();
        return MesaMapper.ToMesaDto(mesa);
    }

    public MesaDto? crearMesaDto(CrearMesaDto crearMesaDto)
    {

       MesaModel model = MesaMapper.ToMesaModel(crearMesaDto);
       context.Mesas.AddAsync(model);
       context.SaveChangesAsync();
        return MesaMapper.ToMesaDto(model);
    }
}
