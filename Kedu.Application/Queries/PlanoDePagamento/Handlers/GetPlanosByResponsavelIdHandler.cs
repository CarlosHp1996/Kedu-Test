using MediatR;
using AutoMapper;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.PlanoDePagamento.Handlers;

public class GetPlanosByResponsavelIdHandler : IRequestHandler<GetPlanosByResponsavelIdQuery, Result<List<PlanoDePagamentoResponse>>>
{
    private readonly IPlanoDePagamentoRepository _repository;
    private readonly IMapper _mapper;

    public GetPlanosByResponsavelIdHandler(IPlanoDePagamentoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<PlanoDePagamentoResponse>>> Handle(GetPlanosByResponsavelIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var planos = await _repository.GetByResponsavelIdAsync(request.ResponsavelId);
            var response = _mapper.Map<List<PlanoDePagamentoResponse>>(planos);
            
            return new Result<List<PlanoDePagamentoResponse>>(response)
            {
                Count = response.Count,
                Message = $"{response.Count} planos encontrados para o responsável"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<List<PlanoDePagamentoResponse>>();
            result.WithException($"Erro ao buscar planos do responsável: {ex.Message}");
            return result;
        }
    }
}