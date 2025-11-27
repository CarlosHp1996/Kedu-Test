using Kedu.Domain.Enums;

namespace Kedu.Application.Services.Interfaces;

public interface IPaymentCodeGenerator
{
    string GeneratePaymentCode(MetodoPagamento metodoPagamento, int cobrancaId = 0);
    bool ValidatePaymentCode(string paymentCode, MetodoPagamento metodoPagamento);
}
