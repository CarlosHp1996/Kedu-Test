using Kedu.Domain.Entities;

namespace Kedu.Application.Interfaces;

public interface IResponsavelFinanceiroRepository : IRepository<ResponsavelFinanceiro>
{    
    Task<ResponsavelFinanceiro?> GetWithPlanosAsync(int id);
}