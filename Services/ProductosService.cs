using Microsoft.EntityFrameworkCore;
using JeisonAmparo_AP1_P2.Context;
using JeisonAmparo_AP1_P2.Models;
using System.Linq.Expressions;

namespace JeisonAmparo_AP1_P2.Services;

public class ProductosService(
    IDbContextFactory<Contexto> contextFactory
): Aplicada1.Core.IService<Productos, int>
{
    public async Task<bool> Guardar(Productos producto)
    {
        if (!await Existe(producto.ProductoId))
        {
            var inserto = await Insertar(producto);
            return inserto;
        }
        else
        {
            var modifico = await Modificar(producto);
            return modifico;
        }
    }

    private async Task<bool> Existe(int productoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Productos
            .AnyAsync(p => p.ProductoId == productoId);
    }

    private async Task<bool> Insertar(Productos producto)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Productos.Add(producto);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Productos producto)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(producto);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Productos?> Buscar(int productoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Productos
            .FirstOrDefaultAsync(p => p.ProductoId == productoId);
    }

    public async Task<List<Productos>> GetList(Expression<Func<Productos, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Productos
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Eliminar(int productoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var tieneVentas = await contexto.VentasDetalle
            .AnyAsync(d => d.ProductoId == productoId);
        if (tieneVentas)
            return false;

        var eliminado = await contexto.Productos
            .Where(p => p.ProductoId == productoId)
            .ExecuteDeleteAsync() > 0;
        return eliminado;
    }
}
