using MediatR;
using Kedu.Application.Interfaces;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.CentroDeCusto.Handlers;

public class DeleteCentroDeCustoHandler : IRequestHandler<DeleteCentroDeCustoCommand, Result<bool>>
{
    private readonly ICentroDeCustoRepository _repository;

    public DeleteCentroDeCustoHandler(ICentroDeCustoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(DeleteCentroDeCustoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var centroDeCusto = await _repository.GetByIdAsync(request.Id);
            if (centroDeCusto == null)
            {
                var notFoundResult = new Result<bool>();
                notFoundResult.WithNotFound($"Id {request.Id} do Plano De Pagamento não encontrado");
                return notFoundResult;
            }

            await _repository.DeleteAsync(centroDeCusto);
            
            return new Result<bool>(true)
            {
                Message = "Centro de custo removido com sucesso"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<bool>();
            result.WithException($"Erro ao remover centro de custo: {ex.Message}");
            return result;
        }
    }
}