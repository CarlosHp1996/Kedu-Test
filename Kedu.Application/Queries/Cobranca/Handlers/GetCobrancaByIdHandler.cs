using MediatR;
using AutoMapper;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.Cobranca.Handlers;

public class GetCobrancaByIdHandler : IRequestHandler<GetCobrancaByIdQuery, Result<CobrancaDetailResponse>>
{
    private readonly ICobrancaRepository _repository;
    private readonly IMapper _mapper;

    public GetCobrancaByIdHandler(ICobrancaRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<CobrancaDetailResponse>> Handle(GetCobrancaByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var cobranca = await _repository.GetByIdWithRelatedDataAsync(request.Id);
            if (cobranca == null)
            {
                var notFoundResult = new Result<CobrancaDetailResponse>();
                notFoundResult.WithNotFound($"Id {request.Id} da Cobrança não encontrado");
                return notFoundResult;
            }

            var response = _mapper.Map<CobrancaDetailResponse>(cobranca);
            return new Result<CobrancaDetailResponse>(response)
            {
                Message = "Cobrança encontrada com sucesso"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<CobrancaDetailResponse>();
            result.WithException($"Erro ao buscar cobrança: {ex.Message}");
            return result;
        }
    }
}