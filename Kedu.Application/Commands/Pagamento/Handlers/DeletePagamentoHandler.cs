using MediatR;
using Kedu.Application.Interfaces;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.Pagamento.Handlers;

public class DeletePagamentoHandler : IRequestHandler<DeletePagamentoCommand, Result<bool>>
{
    private readonly IPagamentoRepository _repository;

    public DeletePagamentoHandler(IPagamentoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(DeletePagamentoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var pagamento = await _repository.GetByIdAsync(request.Id);
            if (pagamento == null)
            {
                var notFoundResult = new Result<bool>();
                notFoundResult.WithNotFound($"Id  {request.Id} do Plano De Pagamento não encontrado");
                return notFoundResult;
            }

            await _repository.DeleteAsync(pagamento);
            
            return new Result<bool>(true)
            {
                Message = "Pagamento removido com sucesso"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<bool>();
            result.WithException($"Erro ao remover pagamento: {ex.Message}");
            return result;
        }
    }
}