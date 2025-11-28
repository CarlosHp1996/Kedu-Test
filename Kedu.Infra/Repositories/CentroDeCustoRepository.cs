using Kedu.Application.Interfaces;
using Kedu.Domain.Entities;
using Kedu.Domain.Enums;
using Kedu.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Kedu.Infra.Repositories;

public class CentroDeCustoRepository : BaseRepository<CentroDeCusto>, ICentroDeCustoRepository
{
    public CentroDeCustoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsByNomeAsync(string nome)
    {
        return await _dbSet
            .AnyAsync(c => c.Nome.ToLower() == nome.ToLower());
    }
}