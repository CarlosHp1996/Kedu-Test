using MediatR;
using AutoMapper;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.CentroDeCusto.Handlers;

public class GetAllCentrosDeCustoHandler : IRequestHandler<GetAllCentrosDeCustoQuery, Result<List<CentroDeCustoResponse>>>
{
    private readonly ICentroDeCustoRepository _repository;
    private readonly IMapper _mapper;

    public GetAllCentrosDeCustoHandler(ICentroDeCustoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<CentroDeCustoResponse>>> Handle(GetAllCentrosDeCustoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var centrosDeCusto = await _repository.GetAllAsync();
            var response = _mapper.Map<List<CentroDeCustoResponse>>(centrosDeCusto);
            
            return new Result<List<CentroDeCustoResponse>>(response)
            {
                Count = response.Count,
                Message = $"{response.Count} centros de custo encontrados"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<List<CentroDeCustoResponse>>();
            result.WithException($"Erro ao buscar centros de custo: {ex.Message}");
            return result;
        }
    }
}