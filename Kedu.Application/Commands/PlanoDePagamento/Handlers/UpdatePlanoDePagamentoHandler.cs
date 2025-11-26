using MediatR;
using AutoMapper;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.PlanoDePagamento.Handlers;

public class UpdatePlanoDePagamentoHandler : IRequestHandler<UpdatePlanoDePagamentoCommand, Result<PlanoDePagamentoResponse>>
{
    private readonly IPlanoDePagamentoRepository _planoDePagamentoRepository;
    private readonly ICentroDeCustoRepository _centroDeCustoRepository;
    private readonly IMapper _mapper;

    public UpdatePlanoDePagamentoHandler(
        IPlanoDePagamentoRepository planoDePagamentoRepository,
        ICentroDeCustoRepository centroDeCustoRepository,
        IMapper mapper)
    {
        _planoDePagamentoRepository = planoDePagamentoRepository;
        _centroDeCustoRepository = centroDeCustoRepository;
        _mapper = mapper;
    }

    public async Task<Result<PlanoDePagamentoResponse>> Handle(UpdatePlanoDePagamentoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var plano = await _planoDePagamentoRepository.GetWithCobrancasAsync(request.Id);
            if (plano == null)
            {
                var notFoundResult = new Result<PlanoDePagamentoResponse>();
                notFoundResult.WithNotFound($"Id {request.Id} do Plano De Pagamento não encontrado");
                return notFoundResult;
            }

            var centroDeCusto = await _centroDeCustoRepository.GetByIdAsync(request.CentroDeCustoId);
            if (centroDeCusto == null)
            {
                var centroNotFoundResult = new Result<PlanoDePagamentoResponse>();
                centroNotFoundResult.WithNotFound($"Id {request.CentroDeCustoId} do Centro De Custo não encontrado");
                return centroNotFoundResult;
            }

            plano.CentroDeCustoId = request.CentroDeCustoId;
            await _planoDePagamentoRepository.UpdateAsync(plano);

            var response = _mapper.Map<PlanoDePagamentoResponse>(plano);
            return new Result<PlanoDePagamentoResponse>(response)
            {
                Message = "Plano de pagamento atualizado com sucesso"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<PlanoDePagamentoResponse>();
            result.WithException($"Erro ao atualizar plano de pagamento: {ex.Message}");
            return result;
        }
    }
}