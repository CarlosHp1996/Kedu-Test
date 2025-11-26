using Kedu.Domain.Entities;

namespace Kedu.Application.Interfaces;

public interface IPagamentoRepository : IRepository<Pagamento>
{
    Task<IEnumerable<Pagamento>> GetByCobrancaIdAsync(int cobrancaId);
}