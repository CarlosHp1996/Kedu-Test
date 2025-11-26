using MediatR;
using AutoMapper;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.Cobranca.Handlers;

public class GetAllCobrancasHandler : IRequestHandler<GetAllCobrancasQuery, Result<List<CobrancaResponse>>>
{
    private readonly ICobrancaRepository _repository;
    private readonly IMapper _mapper;

    public GetAllCobrancasHandler(ICobrancaRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<CobrancaResponse>>> Handle(GetAllCobrancasQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var cobrancas = await _repository.GetAllAsync();
            var response = _mapper.Map<List<CobrancaResponse>>(cobrancas);
            
            return new Result<List<CobrancaResponse>>(response)
            {
                Count = response.Count,
                Message = $"{response.Count} cobranças encontradas"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<List<CobrancaResponse>>();
            result.WithException($"Erro ao buscar cobranças: {ex.Message}");
            return result;
        }
    }
}