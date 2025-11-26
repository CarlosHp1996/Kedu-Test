using MediatR;
using AutoMapper;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.Pagamento.Handlers;

public class GetPagamentosByCobrancaIdHandler : IRequestHandler<GetPagamentosByCobrancaIdQuery, Result<List<PagamentoResponse>>>
{
    private readonly IPagamentoRepository _repository;
    private readonly IMapper _mapper;

    public GetPagamentosByCobrancaIdHandler(IPagamentoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<PagamentoResponse>>> Handle(GetPagamentosByCobrancaIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagamentos = await _repository.GetByCobrancaIdAsync(request.CobrancaId);
            var response = _mapper.Map<List<PagamentoResponse>>(pagamentos);
            
            return new Result<List<PagamentoResponse>>(response)
            {
                Count = response.Count,
                Message = $"{response.Count} pagamentos encontrados para a cobrança"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<List<PagamentoResponse>>();
            result.WithException($"Erro ao buscar pagamentos da cobrança: {ex.Message}");
            return result;
        }
    }
}