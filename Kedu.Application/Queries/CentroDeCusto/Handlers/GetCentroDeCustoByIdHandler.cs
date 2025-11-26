using MediatR;
using AutoMapper;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.CentroDeCusto.Handlers;

public class GetCentroDeCustoByIdHandler : IRequestHandler<GetCentroDeCustoByIdQuery, Result<CentroDeCustoResponse>>
{
    private readonly ICentroDeCustoRepository _repository;
    private readonly IMapper _mapper;

    public GetCentroDeCustoByIdHandler(ICentroDeCustoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<CentroDeCustoResponse>> Handle(GetCentroDeCustoByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var centroDeCusto = await _repository.GetByIdAsync(request.Id);
            if (centroDeCusto == null)
            {
                var notFoundResult = new Result<CentroDeCustoResponse>();
                notFoundResult.WithNotFound($"Id {request.Id} do Centro De Custo não encontrado");
                return notFoundResult;
            }

            var response = _mapper.Map<CentroDeCustoResponse>(centroDeCusto);
            return new Result<CentroDeCustoResponse>(response)
            {
                Message = "Centro de custo encontrado com sucesso"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<CentroDeCustoResponse>();
            result.WithException($"Erro ao buscar centro de custo: {ex.Message}");
            return result;
        }
    }
}