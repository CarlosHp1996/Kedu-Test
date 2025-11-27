using Kedu.Domain.Entities;

namespace Kedu.Application.Interfaces;

public interface ICobrancaRepository : IBaseRepository<Cobranca>
{
    Task<IEnumerable<Cobranca>> GetByResponsavelIdAsync(int responsavelId);
    
    Task<int> CountByResponsavelIdAsync(int responsavelId);
    
    Task<Cobranca?> GetByCodigoPagamentoAsync(string codigoPagamento);
    
    Task<IEnumerable<Cobranca>> GetByPlanoDePagamentoIdAsync(int planoId);
}