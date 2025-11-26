using MediatR;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.Pagamento;

public class DeletePagamentoCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}