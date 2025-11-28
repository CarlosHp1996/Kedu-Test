using FluentAssertions;
using Kedu.Domain.Entities;
using Kedu.Domain.Enums;
using Kedu.Test.Fixtures;
using Xunit;

namespace Kedu.Test.Unit.Domain;

public class PlanoDePagamentoTests
{
    [Fact]
    public void PlanoDePagamento_Should_Be_Created_With_Valid_Data()
    {
        // Arrange & Act
        var plano = TestDataBuilder.CreateValidPlano();

        // Assert
        plano.Should().NotBeNull();
        plano.ResponsavelFinanceiroId.Should().Be(1);
        plano.CentroDeCustoId.Should().Be(1);
        plano.Id.Should().Be(1);
    }

    [Fact]
    public void PlanoDePagamento_Should_Have_ValorTotal_Property()
    {
        // Arrange & Act
        var plano = TestDataBuilder.CreateValidPlano();

        // Assert
        plano.ValorTotal.Should().BeGreaterOrEqualTo(0);
    }

    [Fact]
    public void PlanoDePagamento_Should_Have_Relationship_With_ResponsavelFinanceiro()
    {
        // Arrange & Act
        var plano = TestDataBuilder.CreateValidPlano();

        // Assert
        plano.ResponsavelFinanceiro.Should().NotBeNull();
        plano.ResponsavelFinanceiro.Nome.Should().Be("Carlos Henrique");
    }

    [Fact]
    public void PlanoDePagamento_Should_Have_Relationship_With_CentroDeCusto()
    {
        // Arrange & Act
        var plano = TestDataBuilder.CreateValidPlano();

        // Assert
        plano.CentroDeCusto.Should().NotBeNull();
    }

    [Fact]
    public void PlanoDePagamento_Should_Initialize_With_Empty_Cobrancas_Collection()
    {
        // Arrange & Act
        var plano = new PlanoDePagamento();

        // Assert
        plano.Cobrancas.Should().NotBeNull();
        plano.Cobrancas.Should().BeEmpty();
    }
}