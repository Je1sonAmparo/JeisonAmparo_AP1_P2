using Microsoft.EntityFrameworkCore;
using JeisonAmparo_AP1_P2.Context;
using JeisonAmparo_AP1_P2.Models;
using System.Linq.Expressions;
using Aplicada1.Core;

namespace JeisonAmparo_AP1_P2.Services;

public class ModelosService(
IDbContextFactory<Contexto> contextFactory
) : Aplicada1.Core.IServiceResult<Modelo1, int>
{
    public Task<Result<Modelo1?>> Buscar(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> Eliminar(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<Modelo1>>> GetList(Expression<Func<Modelo1, bool>> criterio)
    {
        throw new NotImplementedException();
    }

    public Task<Result> Guardar(Modelo1 entidad)
    {
        throw new NotImplementedException();
    }
}
