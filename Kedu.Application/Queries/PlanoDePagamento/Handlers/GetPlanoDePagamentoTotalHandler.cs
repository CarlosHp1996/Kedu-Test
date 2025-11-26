using MediatR;
using AutoMapper;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.PlanoDePagamento.Handlers;

public class GetPlanoDePagamentoTotalHandler : IRequestHandler<GetPlanoDePagamentoTotalQuery, Result<decimal>>
{
    private readonly IPlanoDePagamentoRepository _repository;

    public GetPlanoDePagamentoTotalHandler(IPlanoDePagamentoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<decimal>> Handle(GetPlanoDePagamentoTotalQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Verificar se o plano existe
            var planoExists = await _repository.GetByIdAsync(request.Id);
            if (planoExists == null)
            {
                var notFoundResult = new Result<decimal>();
                notFoundResult.WithNotFound($"Id {request.Id} do Plano De Pagamento não encontrado");
                return notFoundResult;
            }

            var total = await _repository.GetTotalValueAsync(request.Id);
            return new Result<decimal>(total)
            {
                Message = $"Valor total do plano: {total:C}"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<decimal>();
            result.WithException($"Erro ao calcular valor total do plano: {ex.Message}");
            return result;
        }
    }
}