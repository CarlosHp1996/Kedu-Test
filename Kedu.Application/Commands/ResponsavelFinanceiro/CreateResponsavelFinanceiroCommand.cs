using MediatR;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.ResponsavelFinanceiro;

public class CreateResponsavelFinanceiroCommand : IRequest<Result<ResponsavelFinanceiroResponse>>
{
    public required string Nome { get; set; }
}