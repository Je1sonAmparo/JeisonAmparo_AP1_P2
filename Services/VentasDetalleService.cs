using Microsoft.EntityFrameworkCore;
using JeisonAmparo_AP1_P2.Context;
using JeisonAmparo_AP1_P2.Models;
using System.Linq.Expressions;

namespace JeisonAmparo_AP1_P2.Services;

public class VentasDetalleService(
    IDbContextFactory<Contexto> contextFactory
): Aplicada1.Core.IService<VentasDetalle, int>
{
    public async Task<bool> Guardar(VentasDetalle detalle)
    {
        if (!await Existe(detalle.DetalleId))
        {
            var inserto = await Insertar(detalle);
            return inserto;
        }
        else
        {
            var modifico = await Modificar(detalle);
            return modifico;
        }
    }

    private async Task<bool> Existe(int detalleId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.VentasDetalle
            .AnyAsync(d => d.DetalleId == detalleId);
    }

    private async Task<bool> Insertar(VentasDetalle detalle)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.VentasDetalle.Add(detalle);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(VentasDetalle detalle)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(detalle);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<VentasDetalle?> Buscar(int detalleId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.VentasDetalle
            .FirstOrDefaultAsync(d => d.DetalleId == detalleId);
    }

    public async Task<List<VentasDetalle>> GetList(Expression<Func<VentasDetalle, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.VentasDetalle
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Eliminar(int detalleId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var eliminado = await contexto.VentasDetalle
            .Where(d => d.DetalleId == detalleId)
            .ExecuteDeleteAsync() > 0;
        return eliminado;
    }
}
