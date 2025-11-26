using MediatR;
using AutoMapper;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.Pagamento.Handlers;

public class GetAllPagamentosHandler : IRequestHandler<GetAllPagamentosQuery, Result<List<PagamentoResponse>>>
{
    private readonly IPagamentoRepository _repository;
    private readonly IMapper _mapper;

    public GetAllPagamentosHandler(IPagamentoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<PagamentoResponse>>> Handle(GetAllPagamentosQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagamentos = await _repository.GetAllAsync();
            var response = _mapper.Map<List<PagamentoResponse>>(pagamentos);
            
            return new Result<List<PagamentoResponse>>(response)
            {
                Count = response.Count,
                Message = $"{response.Count} pagamentos encontrados"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<List<PagamentoResponse>>();
            result.WithException($"Erro ao buscar pagamentos: {ex.Message}");
            return result;
        }
    }
}