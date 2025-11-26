using MediatR;
using AutoMapper;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.ResponsavelFinanceiro.Handlers;

public class GetAllResponsaveisFinanceirosHandler : IRequestHandler<GetAllResponsaveisFinanceirosQuery, Result<List<ResponsavelFinanceiroResponse>>>
{
    private readonly IResponsavelFinanceiroRepository _repository;
    private readonly IMapper _mapper;

    public GetAllResponsaveisFinanceirosHandler(IResponsavelFinanceiroRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<ResponsavelFinanceiroResponse>>> Handle(GetAllResponsaveisFinanceirosQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var responsaveis = await _repository.GetAllAsync();
            var response = _mapper.Map<List<ResponsavelFinanceiroResponse>>(responsaveis);
            
            return new Result<List<ResponsavelFinanceiroResponse>>(response)
            {
                Count = response.Count,
                Message = $"{response.Count} responsáveis financeiros encontrados"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<List<ResponsavelFinanceiroResponse>>();
            result.WithException($"Erro ao buscar responsáveis financeiros: {ex.Message}");
            return result;
        }
    }
}