using MediatR;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.ResponsavelFinanceiro;

public class GetResponsavelFinanceiroDetailQuery : IRequest<Result<ResponsavelFinanceiroDetailResponse>>
{
    public int Id { get; set; }
}