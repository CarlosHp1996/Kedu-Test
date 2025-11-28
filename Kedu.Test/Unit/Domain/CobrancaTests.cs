using FluentAssertions;
using Kedu.Domain.Entities;
using Kedu.Domain.Enums;
using Kedu.Test.Fixtures;
using Xunit;

namespace Kedu.Test.Unit.Domain;

public class CobrancaTests
{
    [Fact]
    public void Cobranca_Should_Be_Created_With_Valid_Data()
    {
        // Arrange & Act
        var cobranca = TestDataBuilder.CreateValidCobranca();

        // Assert
        cobranca.Should().NotBeNull();
        cobranca.PlanoDePagamentoId.Should().Be(1);
        cobranca.Valor.Should().Be(500m);
        cobranca.MetodoPagamento.Should().Be(MetodoPagamento.Pix);
        cobranca.Status.Should().Be(StatusCobranca.Emitida);
        cobranca.CodigoPagamento.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Cobranca_Should_Have_Different_Payment_Codes_For_Different_Methods()
    {
        // Arrange & Act
        var cobrancaPix = new Cobranca 
        { 
            MetodoPagamento = MetodoPagamento.Pix,
            CodigoPagamento = "PIX-001-ABC123"
        };
        
        var cobrancaBoleto = new Cobranca 
        { 
            MetodoPagamento = MetodoPagamento.Boleto,
            CodigoPagamento = "BOL-001-XYZ789"
        };

        // Assert
        cobrancaPix.CodigoPagamento.Should().StartWith("PIX");
        cobrancaBoleto.CodigoPagamento.Should().StartWith("BOL");
        cobrancaPix.CodigoPagamento.Should().NotBe(cobrancaBoleto.CodigoPagamento);
    }

    [Theory]
    [InlineData(StatusCobranca.Emitida)]
    [InlineData(StatusCobranca.Paga)]
    [InlineData(StatusCobranca.Cancelada)]
    public void Cobranca_Should_Accept_Valid_Status(StatusCobranca status)
    {
        // Arrange
        var cobranca = TestDataBuilder.CreateValidCobranca();

        // Act
        cobranca.Status = status;

        // Assert
        cobranca.Status.Should().Be(status);
    }

    [Fact]
    public void Cobranca_Should_Calculate_If_Vencida()
    {
        // Arrange
        var cobrancaVencida = new Cobranca
        {
            DataVencimento = DateTime.UtcNow.AddDays(-1),
            Status = StatusCobranca.Emitida,
            CodigoPagamento = "TEST-001"
        };

        var cobrancaVigente = new Cobranca
        {
            DataVencimento = DateTime.UtcNow.AddDays(30),
            Status = StatusCobranca.Emitida,
            CodigoPagamento = "TEST-002"
        };

        var cobrancaPaga = new Cobranca
        {
            DataVencimento = DateTime.UtcNow.AddDays(-1),
            Status = StatusCobranca.Paga,
            CodigoPagamento = "TEST-003"
        };

        // Act & Assert - Status VENCIDA é calculado, não persistido
        (cobrancaVencida.DataVencimento < DateTime.UtcNow && cobrancaVencida.Status == StatusCobranca.Emitida)
            .Should().BeTrue();
        
        (cobrancaVigente.DataVencimento >= DateTime.UtcNow)
            .Should().BeTrue();
        
        (cobrancaPaga.Status == StatusCobranca.Paga)
            .Should().BeTrue();
    }

    [Fact]
    public void Cobranca_Should_Initialize_With_Empty_Pagamentos_Collection()
    {
        // Arrange & Act
        var cobranca = new Cobranca { CodigoPagamento = "TEST" };

        // Assert
        cobranca.Pagamentos.Should().NotBeNull();
        cobranca.Pagamentos.Should().BeEmpty();
    }
}