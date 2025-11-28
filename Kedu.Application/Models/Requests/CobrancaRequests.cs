using Kedu.Domain.Enums;

namespace Kedu.Application.Models.Requests;

public class UpdateCobrancaRequest
{
    public decimal? Valor { get; set; }
    public DateTime? DataVencimento { get; set; }
    public MetodoPagamento? MetodoPagamento { get; set; }
}

public class CancelCobrancaRequest
{
    public string Motivo { get; set; } = string.Empty;
}