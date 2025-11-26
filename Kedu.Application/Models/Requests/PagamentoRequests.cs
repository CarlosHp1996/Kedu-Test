namespace Kedu.Application.Models.Requests;

public class CreatePagamentoRequest
{
    public int CobrancaId { get; set; }
    public decimal Valor { get; set; }
    public DateTime? DataPagamento { get; set; }
}