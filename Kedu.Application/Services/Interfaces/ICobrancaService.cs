using Kedu.Domain.Entities;
using Kedu.Domain.Enums;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Services.Interfaces;

public interface ICobrancaService
{
    Task<Result<Cobranca>> CreateCobrancaAsync(Cobranca cobranca);
    Task<Result<Cobranca>> UpdateCobrancaAsync(int id, Cobranca cobranca);
    Task<Result> CancelCobrancaAsync(int id);
    Task<Result> DeleteCobrancaAsync(int id);
    Task<Result<Cobranca>> GetByIdAsync(int id);
    Task<Result<IEnumerable<Cobranca>>> GetAllAsync();
    Task<Result<IEnumerable<Cobranca>>> GetByResponsavelIdAsync(int responsavelId);
    Task<Result<int>> GetQuantidadeByResponsavelIdAsync(int responsavelId);
    Task<Result<Pagamento>> ProcessPaymentAsync(int cobrancaId, decimal valor);
    Task<Result<bool>> CanProcessPaymentAsync(int cobrancaId);
    Task<Result<bool>> CobrancaExistsAsync(int id);
    Task<StatusCobranca> CalculateStatusAsync(int cobrancaId);
}
