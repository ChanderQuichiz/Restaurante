using System;
using application.Models;
using Microsoft.EntityFrameworkCore;

namespace application.Data;

public class DbAppContext : DbContext
{
    public DbAppContext(DbContextOptions<DbAppContext> options) : base(options)
    {
    }
    public DbSet<PlatoModel> Platos { get; set; }
    public DbSet<MesaModel> Mesas { get; set; }
}
