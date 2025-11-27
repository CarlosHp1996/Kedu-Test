using FluentValidation;
using Kedu.Application.Commands.PlanoDePagamento;
using Kedu.Domain.Enums;

namespace Kedu.Application.Validators;

public class CreatePlanoDePagamentoCommandValidator : AbstractValidator<CreatePlanoDePagamentoCommand>
{
    public CreatePlanoDePagamentoCommandValidator()
    {
        RuleFor(x => x.ResponsavelId)
            .GreaterThan(0)
            .WithMessage("ResponsavelId deve ser maior que zero");

        RuleFor(x => x.CentroDeCusto)
            .GreaterThan(0)
            .WithMessage("CentroDeCusto deve ser maior que zero");

        RuleFor(x => x.Cobrancas)
            .NotEmpty()
            .WithMessage("Plano deve ter pelo menos uma cobrança")
            .Must(cobrancas => cobrancas.Count > 0)
            .WithMessage("Plano deve ter pelo menos uma cobrança");

        RuleForEach(x => x.Cobrancas).ChildRules(cobranca =>
        {
            cobranca.RuleFor(c => c.Valor)
                .GreaterThan(0)
                .WithMessage("Valor da cobrança deve ser maior que zero");

            cobranca.RuleFor(c => c.DataVencimento)
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Data de vencimento não pode ser no passado");

            cobranca.RuleFor(c => c.MetodoPagamento)
                .Must(method => method == MetodoPagamento.Boleto || method == MetodoPagamento.Pix)
                .WithMessage("Método de pagamento deve ser BOLETO ou PIX");
        });
    }
}