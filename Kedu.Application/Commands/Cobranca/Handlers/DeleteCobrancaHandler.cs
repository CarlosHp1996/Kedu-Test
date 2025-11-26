using MediatR;
using Kedu.Application.Interfaces;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.Cobranca.Handlers;

public class DeleteCobrancaHandler : IRequestHandler<DeleteCobrancaCommand, Result<bool>>
{
    private readonly ICobrancaRepository _repository;

    public DeleteCobrancaHandler(ICobrancaRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(DeleteCobrancaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var cobranca = await _repository.GetByIdAsync(request.Id);
            if (cobranca == null)
            {
                var notFoundResult = new Result<bool>();
                notFoundResult.WithNotFound($"Id {request.Id} da Cobrança não encontrado");
                return notFoundResult;
            }

            await _repository.DeleteAsync(cobranca);
            
            return new Result<bool>(true)
            {
                Message = "Cobrança removida com sucesso"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<bool>();
            result.WithException($"Erro ao remover cobrança: {ex.Message}");
            return result;
        }
    }
}