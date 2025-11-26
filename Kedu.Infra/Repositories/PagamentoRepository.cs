using Kedu.Application.Interfaces;
using Kedu.Domain.Entities;
using Kedu.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Kedu.Infra.Repositories;

public class PagamentoRepository : BaseRepository<Pagamento>, IPagamentoRepository
{
    public PagamentoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Pagamento>> GetByCobrancaIdAsync(int cobrancaId)
    {
        return await _dbSet
            .Include(p => p.Cobranca)
                .ThenInclude(c => c.PlanoDePagamento)
            .Where(p => p.CobrancaId == cobrancaId)
            .OrderByDescending(p => p.DataPagamento)
            .ToListAsync();
    }
}