using MediatR;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.ResponsavelFinanceiro;

public class GetResponsavelFinanceiroByIdQuery : IRequest<Result<ResponsavelFinanceiroResponse>>
{
    public int Id { get; set; }
}