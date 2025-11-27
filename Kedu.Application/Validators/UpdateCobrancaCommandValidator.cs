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

        RuleFor(x => x.Valor)
            .GreaterThan(0)
            .WithMessage("Valor da cobrança deve ser maior que zero");

        RuleFor(x => x.DataVencimento)
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Data de vencimento não pode ser no passado");

        RuleFor(x => x.MetodoPagamento)
            .Must(method => method == MetodoPagamento.Boleto || method == MetodoPagamento.Pix)
            .WithMessage("Método de pagamento deve ser BOLETO ou PIX");
    }
}