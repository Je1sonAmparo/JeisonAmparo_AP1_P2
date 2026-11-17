using Microsoft.EntityFrameworkCore;
using JeisonAmparo_AP1_P2.Context;
using JeisonAmparo_AP1_P2.Models;
using System.Linq.Expressions;

namespace JeisonAmparo_AP1_P2.Services;

public class VentasService(
    IDbContextFactory<Contexto> contextFactory
): Aplicada1.Core.IService<Ventas, int>
{
    public async Task<bool> Guardar(Ventas venta)
    {
        if (venta.Detalles.Count == 0 || venta.Detalles.Any(d => d.Cantidad <= 0))
            return false;

        if (!await Existe(venta.VentaId))
        {
            var inserto = await Insertar(venta);
            return inserto;
        }
        else
        {
            var modifico = await Modificar(venta);
            return modifico;
        }
    }

    private async Task<bool> Existe(int ventaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Ventas
            .AnyAsync(v => v.VentaId == ventaId);
    }

    private async Task<bool> Insertar(Ventas venta)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        if (!await AfectarProductos(contexto, venta, TipoOperacion.Resta))
            return false;
        contexto.Ventas.Add(venta);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Ventas venta)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var anterior = await contexto.Ventas
            .Include(v => v.Detalles)
            .AsNoTracking()
            .SingleAsync(v => v.VentaId == venta.VentaId);

        if (!await AfectarProductos(contexto, anterior, TipoOperacion.Suma))
            return false;
        if (!await AfectarProductos(contexto, venta, TipoOperacion.Resta))
            return false;

        foreach (var detalle in anterior.Detalles)
        {
            if (!venta.Detalles.Any(d => d.DetalleId == detalle.DetalleId))
            {
                detalle.Venta = null!;
                contexto.VentasDetalle.Remove(detalle);
            }
        }

        foreach (var detalle in venta.Detalles)
        {
            detalle.VentaId = venta.VentaId;
            detalle.Venta = venta;
            detalle.Producto = null!;
        }

        contexto.Update(venta);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Ventas?> Buscar(int ventaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Ventas
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(v => v.VentaId == ventaId);
    }

    public async Task<List<Ventas>> GetList(Expression<Func<Ventas, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Ventas
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Eliminar(int ventaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var venta = await contexto.Ventas
            .Include(v => v.Detalles)
            .FirstOrDefaultAsync(v => v.VentaId == ventaId);
        if (venta == null)
            return false;

        if (!await AfectarProductos(contexto, venta, TipoOperacion.Suma))
            return false;
        contexto.VentasDetalle.RemoveRange(venta.Detalles);
        contexto.Ventas.Remove(venta);
        var eliminado = await contexto.SaveChangesAsync() > 0;
        return eliminado;
    }

    private async Task<bool> AfectarProductos(Contexto contexto, Ventas venta, TipoOperacion tipoOperacion)
    {
        foreach (var detalle in venta.Detalles)
        {
            var producto = await contexto.Productos
                .SingleOrDefaultAsync(p => p.ProductoId == detalle.ProductoId);

            if (producto == null)
                return false;

            if (tipoOperacion == TipoOperacion.Resta)
            {
                if (detalle.Cantidad > producto.Stock)
                    return false;

                producto.Stock -= detalle.Cantidad;
            }
            else
                producto.Stock += detalle.Cantidad;
        }

        return true;
    }
}

public enum TipoOperacion
{
    Suma = 1,
    Resta = 2
}

