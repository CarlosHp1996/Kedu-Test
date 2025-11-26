namespace Kedu.Domain.Entities;

public class Pagamento
{
    public int Id { get; set; }
    public int CobrancaId { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataPagamento { get; set; }

    public virtual Cobranca Cobranca { get; set; } = null!;
}