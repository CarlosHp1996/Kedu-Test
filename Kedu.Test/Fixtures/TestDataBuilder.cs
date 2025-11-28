using Kedu.Domain.Entities;
using Kedu.Domain.Enums;

namespace Kedu.Test.Fixtures;

public static class TestDataBuilder
{
    public static ResponsavelFinanceiro CreateValidResponsavel()
    {
        return new ResponsavelFinanceiro
        {
            Id = 1,
            Nome = "Carlos Henrique"
        };
    }

    public static CentroDeCusto CreateValidCentroDeCusto()
    {
        return new CentroDeCusto
        {
            Id = 1,
            Nome = "Matrícula",
            Tipo = TipoCentroDeCusto.Matricula
        };
    }

    public static PlanoDePagamento CreateValidPlano()
    {
        var responsavel = CreateValidResponsavel();
        var centroDeCusto = CreateValidCentroDeCusto();
        
        return new PlanoDePagamento
        {
            Id = 1,
            ResponsavelFinanceiroId = 1,
            CentroDeCustoId = 1,
            ResponsavelFinanceiro = responsavel,
            CentroDeCusto = centroDeCusto,
            Cobrancas = new List<Cobranca>()
        };
    }

    public static Cobranca CreateValidCobranca(int planoId = 1)
    {
        return new Cobranca
        {
            Id = 1,
            PlanoDePagamentoId = planoId,
            Valor = 500m,
            DataVencimento = DateTime.UtcNow.AddDays(30),
            MetodoPagamento = MetodoPagamento.Pix,
            Status = StatusCobranca.Emitida,
            CodigoPagamento = "PIX-001-ABC123"
        };
    }

    public static Pagamento CreateValidPagamento(int cobrancaId = 1)
    {
        return new Pagamento
        {
            Id = 1,
            CobrancaId = cobrancaId,
            Valor = 500m,
            DataPagamento = DateTime.UtcNow
        };
    }

    public static List<Cobranca> CreateCobrancasForPlano(int planoId, int quantidade = 2)
    {
        var cobrancas = new List<Cobranca>();
        
        for (int i = 0; i < quantidade; i++)
        {
            cobrancas.Add(new Cobranca
            {
                Id = i + 1,
                PlanoDePagamentoId = planoId,
                Valor = 500m,
                DataVencimento = DateTime.UtcNow.AddDays(30 * (i + 1)),
                MetodoPagamento = i % 2 == 0 ? MetodoPagamento.Pix : MetodoPagamento.Boleto,
                Status = StatusCobranca.Emitida,
                CodigoPagamento = $"{(i % 2 == 0 ? "PIX" : "BOL")}-{i + 1:D3}-{Guid.NewGuid().ToString()[..6].ToUpper()}"
            });
        }

        return cobrancas;
    }
}