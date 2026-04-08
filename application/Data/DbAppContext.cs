using Microsoft.EntityFrameworkCore;
using application.Models;

namespace application.Data;

public class DbAppContext : DbContext
{
    public DbAppContext(DbContextOptions<DbAppContext> options) : base(options)
    {
    }

    public DbSet<UsuarioModel> Usuarios { get; set; }
    public DbSet<MesaModel> Mesas { get; set; }
    public DbSet<PlatoModel> Platos { get; set; }
    public DbSet<PedidoModel> Pedidos { get; set; }
    public DbSet<DetallePedidoModel> DetallePedidos { get; set; }
    public DbSet<PagoModel> Pagos { get; set; }
    public DbSet<SolicitudModel> Solicitudes { get; set; }
}