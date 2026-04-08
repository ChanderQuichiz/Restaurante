using Microsoft.EntityFrameworkCore;
using application.Data;
using application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IMesaService, MesaService>();
builder.Services.AddScoped<IPlatoService, PlatoService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddDbContext<DbAppContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();