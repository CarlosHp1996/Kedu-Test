using Kedu.Domain.Enums;

namespace Kedu.Application.Models.Requests;

public class CentroDeCustoRequest
{
    public required string Nome { get; set; }
    public TipoCentroDeCusto Tipo { get; set; }
}