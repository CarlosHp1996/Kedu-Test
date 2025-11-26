using Kedu.Application.Interfaces;
using Kedu.Domain.Entities;
using Kedu.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Kedu.Infra.Repositories;

public class PlanoDePagamentoRepository : BaseRepository<PlanoDePagamento>, IPlanoDePagamentoRepository
{
    public PlanoDePagamentoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PlanoDePagamento?> GetWithCobrancasAsync(int id)
    {
        return await _dbSet
            .Include(p => p.ResponsavelFinanceiro)
            .Include(p => p.CentroDeCusto)
            .Include(p => p.Cobrancas)
                .ThenInclude(c => c.Pagamentos)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<PlanoDePagamento>> GetByResponsavelIdAsync(int responsavelId)
    {
        return await _dbSet
            .Include(p => p.CentroDeCusto)
            .Include(p => p.Cobrancas)
            .Where(p => p.ResponsavelFinanceiroId == responsavelId)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalValueAsync(int planoId)
    {
        var plano = await _dbSet
            .Include(p => p.Cobrancas)
            .FirstOrDefaultAsync(p => p.Id == planoId);

        return plano?.Cobrancas.Sum(c => c.Valor) ?? 0;
    }
}