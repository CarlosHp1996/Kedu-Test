using Kedu.Domain.Entities;
using Kedu.Domain.Enums;

namespace Kedu.Application.Interfaces;

public interface ICentroDeCustoRepository : IBaseRepository<CentroDeCusto>
{
    Task<CentroDeCusto?> GetByTipoAsync(TipoCentroDeCusto tipo);
    Task<bool> ExistsByNomeAsync(string nome);
}