using Microsoft.EntityFrameworkCore;
using JeisonAmparo_AP1_P2.Models;

namespace JeisonAmparo_AP1_P2.Context;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options)
    {
    }

    public DbSet<Productos> Productos { get; set; }
    public DbSet<Ventas> Ventas { get; set; }
    public DbSet<VentasDetalle> VentasDetalle { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Productos>().HasData(
            new Productos
            {
                ProductoId = 1,
                Nombre = "Café embotellado de vainilla",
                Precio = 150,
                Stock = 10
            },
            new Productos
            {
                ProductoId = 2,
                Nombre = "Café embotellado de mocha",
                Precio = 160,
                Stock = 10
            },
            new Productos
            {
                ProductoId = 3,
                Nombre = "Café embotellado de caramelo",
                Precio = 165,
                Stock = 10
            },
            new Productos
            {
                ProductoId = 4,
                Nombre = "Café embotellado latte",
                Precio = 140,
                Stock = 10
            },
            new Productos
            {
                ProductoId = 5,
                Nombre = "Café embotellado espresso",
                Precio = 135,
                Stock = 10
            }
        );
    }
}
