using MediatR;
using AutoMapper;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.Cobranca.Handlers;

public class GetCobrancasByResponsavelIdHandler : IRequestHandler<GetCobrancasByResponsavelIdQuery, Result<CobrancaListResponse>>
{
    private readonly ICobrancaRepository _repository;
    private readonly IMapper _mapper;

    public GetCobrancasByResponsavelIdHandler(ICobrancaRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<CobrancaListResponse>> Handle(GetCobrancasByResponsavelIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var cobrancas = await _repository.GetByResponsavelIdAsync(request.ResponsavelId);
            var cobrancasResponse = _mapper.Map<List<CobrancaDetailResponse>>(cobrancas);

            var response = new CobrancaListResponse
            {
                Cobrancas = cobrancasResponse,
                TotalCount = cobrancasResponse.Count,
                ValorTotal = cobrancasResponse.Sum(c => c.Valor)
            };

            return new Result<CobrancaListResponse>(response)
            {
                Count = response.TotalCount,
                Message = $"{response.TotalCount} cobranças encontradas para o responsável. Valor total: {response.ValorTotal:C}"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<CobrancaListResponse>();
            result.WithException($"Erro ao buscar cobranças do responsável: {ex.Message}");
            return result;
        }
    }
}