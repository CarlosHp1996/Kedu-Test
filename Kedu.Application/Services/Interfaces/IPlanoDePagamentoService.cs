using Kedu.Domain.Entities;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Services.Interfaces;

public interface IPlanoDePagamentoService
{
    Task<Result<PlanoDePagamento>> CreatePlanoDePagamentoAsync(PlanoDePagamento plano);
    Task<Result<PlanoDePagamento>> UpdatePlanoDePagamentoAsync(int id, PlanoDePagamento plano);
    Task<Result> DeletePlanoDePagamentoAsync(int id);
    Task<Result<PlanoDePagamento>> GetByIdAsync(int id);
    Task<Result<IEnumerable<PlanoDePagamento>>> GetAllAsync();
    Task<Result<IEnumerable<PlanoDePagamento>>> GetByResponsavelIdAsync(int responsavelId);
    Task<Result<decimal>> GetTotalAsync(int id);
    Task<Result<bool>> CanDeletePlanoAsync(int id);
    Task<Result<bool>> PlanoExistsAsync(int id);
}
