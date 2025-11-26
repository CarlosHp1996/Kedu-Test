using MediatR;
using Kedu.Application.Interfaces;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.ResponsavelFinanceiro.Handlers;

public class DeleteResponsavelFinanceiroHandler : IRequestHandler<DeleteResponsavelFinanceiroCommand, Result<bool>>
{
    private readonly IResponsavelFinanceiroRepository _repository;

    public DeleteResponsavelFinanceiroHandler(IResponsavelFinanceiroRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(DeleteResponsavelFinanceiroCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var responsavel = await _repository.GetByIdAsync(request.Id);
            if (responsavel == null)
            {
                var notFoundResult = new Result<bool>();
                notFoundResult.WithNotFound($"Id {request.Id} do Responsavel Financeiro não encontrado");
                return notFoundResult;
            }

            await _repository.DeleteAsync(responsavel);
            
            return new Result<bool>(true)
            {
                Message = "Responsável financeiro removido com sucesso"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<bool>();
            result.WithException($"Erro ao remover responsável financeiro: {ex.Message}");
            return result;
        }
    }
}