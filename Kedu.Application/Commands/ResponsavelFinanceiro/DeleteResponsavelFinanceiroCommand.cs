using MediatR;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.ResponsavelFinanceiro;

public class DeleteResponsavelFinanceiroCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}