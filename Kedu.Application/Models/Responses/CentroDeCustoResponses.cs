using Kedu.Domain.Enums;

namespace Kedu.Application.Models.Responses;

public class CentroDeCustoResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public TipoCentroDeCusto Tipo { get; set; }
    public string TipoDescricao { get; set; } = string.Empty;
}