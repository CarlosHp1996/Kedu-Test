using Kedu.Domain.Enums;

namespace Kedu.Domain.Entities;

public class CentroDeCusto
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public string? Descricao { get; set; }
    public TipoCentroDeCusto Tipo { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }

    public virtual ICollection<PlanoDePagamento> PlanosDePagamento { get; set; } = [];
}