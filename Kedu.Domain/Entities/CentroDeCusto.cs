using Kedu.Domain.Enums;

namespace Kedu.Domain.Entities;

public class CentroDeCusto
{
    public int Id { get; set; }
    public required string Nome { get; set; }

    public virtual ICollection<PlanoDePagamento> PlanosDePagamento { get; set; } = [];
}