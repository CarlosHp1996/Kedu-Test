using Kedu.Application.Interfaces;
using Kedu.Domain.Entities;
using Kedu.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Kedu.Infra.Repositories;

public class CobrancaRepository : BaseRepository<Cobranca>, ICobrancaRepository
{
    public CobrancaRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Cobranca>> GetByResponsavelIdAsync(int responsavelId)
    {
        return await _dbSet
            .Include(c => c.PlanoDePagamento)
                .ThenInclude(p => p.CentroDeCusto)
            .Include(c => c.PlanoDePagamento)
                .ThenInclude(p => p.ResponsavelFinanceiro)
            .Include(c => c.Pagamentos)
            .Where(c => c.PlanoDePagamento.ResponsavelFinanceiroId == responsavelId)
            .OrderBy(c => c.DataVencimento)
            .ToListAsync();
    }

    public async Task<int> CountByResponsavelIdAsync(int responsavelId)
    {
        return await _dbSet
            .CountAsync(c => c.PlanoDePagamento.ResponsavelFinanceiroId == responsavelId);
    }

    public async Task<Cobranca?> GetByCodigoPagamentoAsync(string codigoPagamento)
    {
        return await _dbSet
            .Include(c => c.PlanoDePagamento)
            .Include(c => c.Pagamentos)
            .FirstOrDefaultAsync(c => c.CodigoPagamento == codigoPagamento);
    }

    public async Task<IEnumerable<Cobranca>> GetByPlanoDePagamentoIdAsync(int planoId)
    {
        return await _dbSet
            .Include(c => c.Pagamentos)
            .Where(c => c.PlanoDePagamentoId == planoId)
            .OrderBy(c => c.DataVencimento)
            .ToListAsync();
    }
}