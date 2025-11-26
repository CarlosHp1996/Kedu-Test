using Kedu.Domain.Entities;

namespace Kedu.Application.Interfaces;

public interface IPagamentoRepository : IBaseRepository<Pagamento>
{
    Task<IEnumerable<Pagamento>> GetByCobrancaIdAsync(int cobrancaId);
}