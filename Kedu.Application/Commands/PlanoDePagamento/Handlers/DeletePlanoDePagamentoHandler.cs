using MediatR;
using Kedu.Application.Interfaces;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.PlanoDePagamento.Handlers;

public class DeletePlanoDePagamentoHandler : IRequestHandler<DeletePlanoDePagamentoCommand, Result<bool>>
{
    private readonly IPlanoDePagamentoRepository _repository;

    public DeletePlanoDePagamentoHandler(IPlanoDePagamentoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(DeletePlanoDePagamentoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var plano = await _repository.GetByIdAsync(request.Id);
            if (plano == null)
            {
                var notFoundResult = new Result<bool>();
                notFoundResult.WithNotFound($"Id {request.Id} do Plano De Pagamento não encontrado");
                return notFoundResult;
            }

            await _repository.DeleteAsync(plano);
            
            return new Result<bool>(true)
            {
                Message = "Plano de pagamento removido com sucesso"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<bool>();
            result.WithException($"Erro ao remover plano de pagamento: {ex.Message}");
            return result;
        }
    }
}