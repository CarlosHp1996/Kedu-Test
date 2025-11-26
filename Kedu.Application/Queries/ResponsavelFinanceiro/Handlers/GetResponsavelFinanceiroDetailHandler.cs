using MediatR;
using AutoMapper;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.ResponsavelFinanceiro.Handlers;

public class GetResponsavelFinanceiroDetailHandler : IRequestHandler<GetResponsavelFinanceiroDetailQuery, Result<ResponsavelFinanceiroDetailResponse>>
{
    private readonly IResponsavelFinanceiroRepository _repository;
    private readonly IMapper _mapper;

    public GetResponsavelFinanceiroDetailHandler(IResponsavelFinanceiroRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<ResponsavelFinanceiroDetailResponse>> Handle(GetResponsavelFinanceiroDetailQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var responsavel = await _repository.GetWithPlanosAsync(request.Id);
            if (responsavel == null)
            {
                var notFoundResult = new Result<ResponsavelFinanceiroDetailResponse>();
                notFoundResult.WithNotFound($"Id {request.Id} do Responsavel Financeiro não encontrado");
                return notFoundResult;
            }

            var response = _mapper.Map<ResponsavelFinanceiroDetailResponse>(responsavel);
            return new Result<ResponsavelFinanceiroDetailResponse>(response)
            {
                Message = "Detalhes do responsável financeiro encontrados com sucesso"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<ResponsavelFinanceiroDetailResponse>();
            result.WithException($"Erro ao buscar detalhes do responsável financeiro: {ex.Message}");
            return result;
        }
    }
}