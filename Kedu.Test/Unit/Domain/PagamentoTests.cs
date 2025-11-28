using FluentAssertions;
using Kedu.Domain.Entities;
using Kedu.Test.Fixtures;
using Xunit;

namespace Kedu.Test.Unit.Domain;

public class PagamentoTests
{
    [Fact]
    public void Pagamento_Should_Be_Created_With_Valid_Data()
    {
        // Arrange & Act
        var pagamento = TestDataBuilder.CreateValidPagamento();

        // Assert
        pagamento.Should().NotBeNull();
        pagamento.CobrancaId.Should().Be(1);
        pagamento.Valor.Should().Be(500m);
        pagamento.DataPagamento.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Pagamento_Should_Have_Relationship_With_Cobranca()
    {
        // Arrange
        var cobranca = TestDataBuilder.CreateValidCobranca();
        var pagamento = TestDataBuilder.CreateValidPagamento(cobranca.Id);

        // Act
        pagamento.Cobranca = cobranca;

        // Assert
        pagamento.Cobranca.Should().NotBeNull();
        pagamento.Cobranca.Id.Should().Be(cobranca.Id);
        pagamento.CobrancaId.Should().Be(cobranca.Id);
    }

    [Theory]
    [InlineData(100.50)]
    [InlineData(999.99)]
    [InlineData(0.01)]
    public void Pagamento_Should_Accept_Valid_Valor(decimal valor)
    {
        // Arrange
        var pagamento = TestDataBuilder.CreateValidPagamento();

        // Act
        pagamento.Valor = valor;

        // Assert
        pagamento.Valor.Should().Be(valor);
    }

    [Fact]
    public void Pagamento_DataPagamento_Should_Be_Set_When_Created()
    {
        // Arrange & Act
        var pagamento = new Pagamento
        {
            CobrancaId = 1,
            Valor = 500m,
            DataPagamento = DateTime.UtcNow
        };

        // Assert
        pagamento.DataPagamento.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Pagamento_Should_Have_Required_Properties()
    {
        // Arrange & Act
        var pagamento = new Pagamento();

        // Assert
        pagamento.Should().NotBeNull();
        pagamento.CobrancaId.Should().Be(0); // Default value
        pagamento.Valor.Should().Be(0m); // Default value
    }
}