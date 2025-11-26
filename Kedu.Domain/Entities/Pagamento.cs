using Kedu.Domain.Enums;

namespace Kedu.Domain.Entities;

public class Pagamento
{
    public int Id { get; set; }
    public int CobrancaId { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataPagamento { get; set; }
    public MetodoPagamento MetodoPagamento { get; set; }
    public string? Observacao { get; set; }
    public string? ComprovanteTransacao { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }

    public virtual Cobranca Cobranca { get; set; } = null!;
}