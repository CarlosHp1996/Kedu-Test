using Kedu.Application.Interfaces;
using Kedu.Domain.Entities;
using Kedu.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Kedu.Infra.Repositories;

public class ResponsavelFinanceiroRepository : BaseRepository<ResponsavelFinanceiro>, IResponsavelFinanceiroRepository
{
    public ResponsavelFinanceiroRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<ResponsavelFinanceiro?> GetWithPlanosAsync(int id)
    {
        return await _dbSet
            .Include(r => r.PlanosDePagamento)
                .ThenInclude(p => p.CentroDeCusto)
            .Include(r => r.PlanosDePagamento)
                .ThenInclude(p => p.Cobrancas)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
}