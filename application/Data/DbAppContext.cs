using System;
using application.Models;
using Microsoft.EntityFrameworkCore;

namespace application.Data;

public class DbAppContext : DbContext
{
    public DbAppContext(DbContextOptions<DbAppContext> options) : base(options)
    {
    }

    public DbSet<UsuarioModel> Usuarios { get; set; }
    public DbSet<PlatoModel> Platos { get; set; }
    public DbSet<PedidoModel> Pedidos { get; set; }
    public DbSet<SolicitudModel> Solicitudes { get; set; }
    public DbSet<DetallePedidoModel> DetallesPedido { get; set; }
    public DbSet<PagoModel> Pagos { get; set; }
    public DbSet<MesaModel> Mesas { get; set; }
}
