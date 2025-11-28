using Kedu.Domain.Entities;
using Kedu.Domain.Enums;

namespace Kedu.Application.Interfaces;

public interface ICentroDeCustoRepository : IBaseRepository<CentroDeCusto>
{
    Task<bool> ExistsByNomeAsync(string nome);
}