using MediatR;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.ResponsavelFinanceiro;

public class GetAllResponsaveisFinanceirosQuery : IRequest<Result<List<ResponsavelFinanceiroResponse>>>
{
}