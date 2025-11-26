namespace Kedu.Domain.Entities;

public class PlanoDePagamento
{
    public int Id { get; set; }
    public int ResponsavelFinanceiroId { get; set; }
    public int CentroDeCustoId { get; set; }
    public decimal ValorTotal => Cobrancas?.Sum(c => c.Valor) ?? 0;

    public virtual ResponsavelFinanceiro ResponsavelFinanceiro { get; set; } = null!;
    public virtual CentroDeCusto CentroDeCusto { get; set; } = null!;
    public virtual ICollection<Cobranca> Cobrancas { get; set; } = [];
}