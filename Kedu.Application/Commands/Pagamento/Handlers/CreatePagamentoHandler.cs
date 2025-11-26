using MediatR;
using AutoMapper;
using Kedu.Domain.Entities;
using Kedu.Domain.Enums;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.Pagamento.Handlers;

public class CreatePagamentoHandler : IRequestHandler<CreatePagamentoCommand, Result<RegistrarPagamentoResponse>>
{
    private readonly IPagamentoRepository _pagamentoRepository;
    private readonly ICobrancaRepository _cobrancaRepository;

    public CreatePagamentoHandler(
        IPagamentoRepository pagamentoRepository,
        ICobrancaRepository cobrancaRepository)
    {
        _pagamentoRepository = pagamentoRepository;
        _cobrancaRepository = cobrancaRepository;
    }

    public async Task<Result<RegistrarPagamentoResponse>> Handle(CreatePagamentoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar cobrança
            var cobranca = await _cobrancaRepository.GetByIdAsync(request.CobrancaId);
            if (cobranca == null)
            {
                var notFoundResult = new Result<RegistrarPagamentoResponse>();
                notFoundResult.WithNotFound($"Cobrança com ID {request.CobrancaId} não encontrada");
                return notFoundResult;
            }

            // Validar regras de negócio
            if (cobranca.Status == StatusCobranca.Cancelada)
            {
                var cancelledResult = new Result<RegistrarPagamentoResponse>();
                cancelledResult.WithError("Não é possível registrar pagamento em cobrança cancelada");
                return cancelledResult;
            }

            if (cobranca.Status == StatusCobranca.Paga)
            {
                var alreadyPaidResult = new Result<RegistrarPagamentoResponse>();
                alreadyPaidResult.WithError("Cobrança já está paga");
                return alreadyPaidResult;
            }

            if (request.Valor <= 0)
            {
                var invalidValueResult = new Result<RegistrarPagamentoResponse>();
                invalidValueResult.WithError("Valor do pagamento deve ser maior que zero");
                return invalidValueResult;
            }

            if (request.Valor > cobranca.Valor)
            {
                var exceedsValueResult = new Result<RegistrarPagamentoResponse>();
                exceedsValueResult.WithError($"Valor do pagamento ({request.Valor:C}) não pode ser maior que o valor da cobrança ({cobranca.Valor:C})");
                return exceedsValueResult;
            }

            // Criar pagamento
            var pagamento = new Domain.Entities.Pagamento
            {
                CobrancaId = request.CobrancaId,
                Valor = request.Valor,
                DataPagamento = request.DataPagamento ?? DateTime.Now
            };

            var createdPagamento = await _pagamentoRepository.AddAsync(pagamento);

            // Atualizar status da cobrança para PAGA (considerando pagamento total)
            cobranca.Status = StatusCobranca.Paga;
            await _cobrancaRepository.UpdateAsync(cobranca);

            var response = new RegistrarPagamentoResponse
            {
                PagamentoId = createdPagamento.Id,
                CobrancaId = cobranca.Id,
                NovoStatusCobranca = StatusCobranca.Paga,
                Mensagem = "Pagamento registrado com sucesso",
                Sucesso = true
            };

            return new Result<RegistrarPagamentoResponse>(response)
            {
                Message = "Pagamento registrado e cobrança marcada como paga com sucesso"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<RegistrarPagamentoResponse>();
            result.WithException($"Erro ao registrar pagamento: {ex.Message}");
            return result;
        }
    }
}