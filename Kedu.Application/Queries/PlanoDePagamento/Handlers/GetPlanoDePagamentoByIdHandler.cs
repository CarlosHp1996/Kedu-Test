using MediatR;
using AutoMapper;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.PlanoDePagamento.Handlers;

public class GetPlanoDePagamentoByIdHandler : IRequestHandler<GetPlanoDePagamentoByIdQuery, Result<PlanoDePagamentoDetailResponse>>
{
    private readonly IPlanoDePagamentoRepository _repository;
    private readonly IMapper _mapper;

    public GetPlanoDePagamentoByIdHandler(IPlanoDePagamentoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<PlanoDePagamentoDetailResponse>> Handle(GetPlanoDePagamentoByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var plano = await _repository.GetWithCobrancasAsync(request.Id);
            if (plano == null)
            {
                var notFoundResult = new Result<PlanoDePagamentoDetailResponse>();
                notFoundResult.WithNotFound($"Id {request.Id} do Plano De Pagamento não encontrado");
                return notFoundResult;
            }

            var response = _mapper.Map<PlanoDePagamentoDetailResponse>(plano);
            return new Result<PlanoDePagamentoDetailResponse>(response)
            {
                Message = "Plano de pagamento encontrado com sucesso"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<PlanoDePagamentoDetailResponse>();
            result.WithException($"Erro ao buscar plano de pagamento: {ex.Message}");
            return result;
        }
    }
}