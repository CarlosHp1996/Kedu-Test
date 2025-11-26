using MediatR;
using AutoMapper;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.ResponsavelFinanceiro.Handlers;

public class GetResponsavelFinanceiroByIdHandler : IRequestHandler<GetResponsavelFinanceiroByIdQuery, Result<ResponsavelFinanceiroResponse>>
{
    private readonly IResponsavelFinanceiroRepository _repository;
    private readonly IMapper _mapper;

    public GetResponsavelFinanceiroByIdHandler(IResponsavelFinanceiroRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<ResponsavelFinanceiroResponse>> Handle(GetResponsavelFinanceiroByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var responsavel = await _repository.GetByIdAsync(request.Id);
            if (responsavel == null)
            {
                var notFoundResult = new Result<ResponsavelFinanceiroResponse>();
                notFoundResult.WithNotFound($"Id {request.Id} do Responsavel Financeiro não encontrado");
                return notFoundResult;
            }

            var response = _mapper.Map<ResponsavelFinanceiroResponse>(responsavel);
            return new Result<ResponsavelFinanceiroResponse>(response)
            {
                Message = "Responsavel Financeiro encontrado com sucesso"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<ResponsavelFinanceiroResponse>();
            result.WithException($"Erro ao buscar responsável financeiro: {ex.Message}");
            return result;
        }
    }
}