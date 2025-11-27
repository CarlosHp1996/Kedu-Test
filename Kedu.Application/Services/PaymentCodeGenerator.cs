using Kedu.Application.Services.Interfaces;
using Kedu.Domain.Enums;

namespace Kedu.Application.Services;

public class PaymentCodeGenerator : IPaymentCodeGenerator
{
    public string GeneratePaymentCode(MetodoPagamento metodoPagamento, int cobrancaId = 0)
    {
        return metodoPagamento switch
        {
            MetodoPagamento.Boleto => $"BOL{DateTime.Now:yyyyMMddHHmmss}{cobrancaId:D6}",
            MetodoPagamento.Pix => $"PIX{Guid.NewGuid().ToString("N")[..16].ToUpper()}",
            _ => $"PAY{Guid.NewGuid().ToString("N")[..16].ToUpper()}"
        };
    }

    public bool ValidatePaymentCode(string paymentCode, MetodoPagamento metodoPagamento)
    {
        if (string.IsNullOrWhiteSpace(paymentCode))
            return false;

        return metodoPagamento switch
        {
            MetodoPagamento.Boleto => paymentCode.StartsWith("BOL") && paymentCode.Length == 23,
            MetodoPagamento.Pix => paymentCode.StartsWith("PIX") && paymentCode.Length == 19,
            _ => paymentCode.StartsWith("PAY") && paymentCode.Length == 19
        };
    }
}