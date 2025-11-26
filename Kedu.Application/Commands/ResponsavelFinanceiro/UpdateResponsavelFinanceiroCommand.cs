using MediatR;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.ResponsavelFinanceiro;

public class UpdateResponsavelFinanceiroCommand : IRequest<Result<ResponsavelFinanceiroResponse>>
{
    public int Id { get; set; }
    public required string Nome { get; set; }
}