using Kedu.Domain.Entities;

namespace Kedu.Application.Interfaces;

public interface IPlanoDePagamentoRepository : IBaseRepository<PlanoDePagamento>
{
    Task<PlanoDePagamento?> GetWithCobrancasAsync(int id);
    
    Task<IEnumerable<PlanoDePagamento>> GetByResponsavelIdAsync(int responsavelId);
    
    Task<decimal> GetTotalValueAsync(int planoId);
    
    Task<PlanoDePagamento?> GetByResponsavelAndCentroDeCustoAsync(int responsavelId, int centroDeCustoId);
}