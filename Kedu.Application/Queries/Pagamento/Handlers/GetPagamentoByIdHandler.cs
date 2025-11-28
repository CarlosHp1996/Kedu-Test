using MediatR;
using AutoMapper;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.Pagamento.Handlers;

public class GetPagamentoByIdHandler : IRequestHandler<GetPagamentoByIdQuery, Result<PagamentoDetailResponse>>
{
    private readonly IPagamentoRepository _repository;
    private readonly IMapper _mapper;

    public GetPagamentoByIdHandler(IPagamentoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<PagamentoDetailResponse>> Handle(GetPagamentoByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagamento = await _repository.GetByIdWithRelatedDataAsync(request.Id);
            if (pagamento == null)
            {
                var notFoundResult = new Result<PagamentoDetailResponse>();
                notFoundResult.WithNotFound($"Id {request.Id} do Pagamento não encontrado");
                return notFoundResult;
            }

            var response = _mapper.Map<PagamentoDetailResponse>(pagamento);
            return new Result<PagamentoDetailResponse>(response)
            {
                Message = "Pagamento encontrado com sucesso"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<PagamentoDetailResponse>();
            result.WithException($"Erro ao buscar pagamento: {ex.Message}");
            return result;
        }
    }
}