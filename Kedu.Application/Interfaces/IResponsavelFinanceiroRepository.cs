using Kedu.Domain.Entities;

namespace Kedu.Application.Interfaces;

public interface IResponsavelFinanceiroRepository : IBaseRepository<ResponsavelFinanceiro>
{    
    Task<ResponsavelFinanceiro?> GetWithPlanosAsync(int id);
}