namespace Kedu.Domain.Entities;

public class ResponsavelFinanceiro
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string Documento { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }

    public virtual ICollection<PlanoDePagamento> PlanosDePagamento { get; set; } = [];
}