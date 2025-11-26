using Kedu.Domain.Enums;

namespace Kedu.Domain.Entities;

public class Cobranca
{
    public int Id { get; set; }
    public int PlanoDePagamentoId { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataVencimento { get; set; }
    public MetodoPagamento MetodoPagamento { get; set; }
    public StatusCobranca Status { get; set; } = StatusCobranca.Emitida;
    public required string CodigoPagamento { get; set; }

    public virtual PlanoDePagamento PlanoDePagamento { get; set; } = null!;
    public virtual ICollection<Pagamento> Pagamentos { get; set; } = [];

    public bool IsVencida => DateTime.Now > DataVencimento && 
                            Status != StatusCobranca.Paga && 
                            Status != StatusCobranca.Cancelada;

    public void GeneratePaymentCode()
    {
        CodigoPagamento = MetodoPagamento switch
        {
            MetodoPagamento.Boleto => $"BOL{DateTime.Now:yyyyMMddHHmmss}{Id:D6}",
            MetodoPagamento.Pix => $"PIX{Guid.NewGuid().ToString("N")[..16]}",
            _ => $"PAY{Guid.NewGuid().ToString("N")[..16]}"
        };
    }
}