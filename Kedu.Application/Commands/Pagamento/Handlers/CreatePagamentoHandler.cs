using Kedu.Application.Models.Responses;
using Kedu.Application.Services.Interfaces;
using Kedu.Domain.Enums;
using Kedu.Domain.Helpers;
using MediatR;

namespace Kedu.Application.Commands.Pagamento.Handlers;

public class CreatePagamentoHandler : IRequestHandler<CreatePagamentoCommand, Result<RegistrarPagamentoResponse>>
{
    private readonly ICobrancaService _cobrancaService;

    public CreatePagamentoHandler(ICobrancaService cobrancaService)
    {
        _cobrancaService = cobrancaService;
    }

    public async Task<Result<RegistrarPagamentoResponse>> Handle(CreatePagamentoCommand request, CancellationToken cancellationToken)
    {
        var result = await _cobrancaService.ProcessPaymentAsync(request.CobrancaId, request.Valor);
        
        if (!result.HasSuccess)
        {
            return new Result<RegistrarPagamentoResponse>
            {
                HasSuccess = result.HasSuccess,
                Errors = result.Errors,
                HttpStatusCode = result.HttpStatusCode,
                Message = result.Message
            };
        }

        var response = new RegistrarPagamentoResponse
        {
            PagamentoId = result.Value.Id,
            CobrancaId = result.Value.CobrancaId,
            NovoStatusCobranca = StatusCobranca.Paga,
            Mensagem = result.Message,
            Sucesso = true
        };

        return new Result<RegistrarPagamentoResponse>(response)
        {
            Message = result.Message
        };
    }
}