using FluentValidation;
using Kedu.Application.Commands.Pagamento;
using Kedu.Application.Services.Interfaces;
using Kedu.Domain.Enums;

namespace Kedu.Application.Validators;

public class CreatePagamentoBusinessRuleValidator : AbstractValidator<CreatePagamentoCommand>
{
    private readonly ICobrancaService _cobrancaService;

    public CreatePagamentoBusinessRuleValidator(ICobrancaService cobrancaService)
    {
        _cobrancaService = cobrancaService;

        RuleFor(x => x.CobrancaId)
            .GreaterThan(0)
            .WithMessage("CobrancaId deve ser maior que zero");

        RuleFor(x => x.Valor)
            .GreaterThan(0)
            .WithMessage("Valor do pagamento deve ser maior que zero");

        // Regra de negócio crítica: Não permitir pagamento em cobrança CANCELADA
        RuleFor(x => x.CobrancaId)
            .MustAsync(async (cobrancaId, cancellation) =>
            {
                var canProcessResult = await _cobrancaService.CanProcessPaymentAsync(cobrancaId);
                return canProcessResult.HasSuccess && canProcessResult.Value;
            })
            .WithMessage("Não é possível registrar pagamento em cobrança CANCELADA ou PAGA")
            .DependentRules(() =>
            {
                // Validação de valor apenas se a cobrança permitir pagamento
                RuleFor(x => new { x.CobrancaId, x.Valor })
                    .MustAsync(async (data, cancellation) =>
                    {
                        var cobrancaResult = await _cobrancaService.GetByIdAsync(data.CobrancaId);
                        if (!cobrancaResult.HasSuccess)
                            return false;

                        // Verificar se valor do pagamento não excede valor da cobrança
                        return data.Valor <= cobrancaResult.Value.Valor;
                    })
                    .WithMessage("Valor do pagamento não pode exceder o valor da cobrança");
            });

        // DataPagamento é opcional, se não informada será DateTime.Now
        When(x => x.DataPagamento.HasValue, () =>
        {
            RuleFor(x => x.DataPagamento!.Value)
                .LessThanOrEqualTo(DateTime.Now.AddDays(1)) // Permite até o dia posterior
                .WithMessage("Data de pagamento não pode ser muito extendida");
        });
    }
}