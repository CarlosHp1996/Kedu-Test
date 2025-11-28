using FluentValidation;
using Kedu.Application.Commands.Cobranca;
using Kedu.Domain.Enums;

namespace Kedu.Application.Validators;

public class UpdateCobrancaCommandValidator : AbstractValidator<UpdateCobrancaCommand>
{
    public UpdateCobrancaCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id deve ser maior que zero");

        // Validar apenas se o valor foi fornecido
        RuleFor(x => x.Valor)
            .GreaterThan(0)
            .When(x => x.Valor.HasValue)
            .WithMessage("Valor da cobrança deve ser maior que zero");

        // Validar apenas se a data foi fornecida
        RuleFor(x => x.DataVencimento)
            .GreaterThanOrEqualTo(DateTime.Today)
            .When(x => x.DataVencimento.HasValue)
            .WithMessage("Data de vencimento não pode ser no passado");

        // Validar apenas se o método foi fornecido
        RuleFor(x => x.MetodoPagamento)
            .Must(method => method == MetodoPagamento.Boleto || method == MetodoPagamento.Pix)
            .When(x => x.MetodoPagamento.HasValue)
            .WithMessage("Método de pagamento deve ser BOLETO ou PIX");

        // Garantir que pelo menos um campo foi fornecido
        RuleFor(x => x)
            .Must(x => x.Valor.HasValue || x.DataVencimento.HasValue || x.MetodoPagamento.HasValue)
            .WithMessage("Pelo menos um campo deve ser fornecido para atualização");
    }
}