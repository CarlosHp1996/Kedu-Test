namespace Kedu.Domain.Entities;

public class ResponsavelFinanceiro
{
    public int Id { get; set; }
    public required string Nome { get; set; }

    public virtual ICollection<PlanoDePagamento> PlanosDePagamento { get; set; } = [];
}