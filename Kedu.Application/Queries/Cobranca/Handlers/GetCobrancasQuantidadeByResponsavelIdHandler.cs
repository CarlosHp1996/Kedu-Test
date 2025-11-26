using MediatR;
using Kedu.Application.Interfaces;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Queries.Cobranca.Handlers;

public class GetCobrancasQuantidadeByResponsavelIdHandler : IRequestHandler<GetCobrancasQuantidadeByResponsavelIdQuery, Result<int>>
{
    private readonly ICobrancaRepository _repository;

    public GetCobrancasQuantidadeByResponsavelIdHandler(ICobrancaRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<int>> Handle(GetCobrancasQuantidadeByResponsavelIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var quantidade = await _repository.CountByResponsavelIdAsync(request.ResponsavelId);
            return new Result<int>(quantidade)
            {
                Message = $"Responsável possui {quantidade} cobranças"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<int>();
            result.WithException($"Erro ao contar cobranças do responsável: {ex.Message}");
            return result;
        }
    }
}