using Kedu.Domain.Entities;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Services.Interfaces;

public interface IResponsavelFinanceiroService
{
    Task<Result<ResponsavelFinanceiro>> CreateResponsavelFinanceiroAsync(ResponsavelFinanceiro responsavel);
    Task<Result<ResponsavelFinanceiro>> UpdateResponsavelFinanceiroAsync(int id, ResponsavelFinanceiro responsavel);
    Task<Result> DeleteResponsavelFinanceiroAsync(int id);
    Task<Result<ResponsavelFinanceiro>> GetByIdAsync(int id);
    Task<Result<IEnumerable<ResponsavelFinanceiro>>> GetAllAsync();
    Task<Result<bool>> CanDeleteResponsavelFinanceiroAsync(int id);
    Task<Result<bool>> ResponsavelExistsAsync(int id);
}
