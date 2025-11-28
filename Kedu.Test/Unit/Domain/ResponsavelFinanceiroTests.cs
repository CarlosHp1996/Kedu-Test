using FluentAssertions;
using Kedu.Domain.Entities;
using Kedu.Test.Fixtures;
using Xunit;

namespace Kedu.Test.Unit.Domain;

public class ResponsavelFinanceiroTests
{
    [Fact]
    public void ResponsavelFinanceiro_Should_Be_Created_With_Valid_Data()
    {
        // Arrange & Act
        var responsavel = TestDataBuilder.CreateValidResponsavel();

        // Assert
        responsavel.Should().NotBeNull();
        responsavel.Nome.Should().Be("Carlos Henrique");
        responsavel.Id.Should().Be(1);
    }

    [Fact]
    public void ResponsavelFinanceiro_Should_Have_PlanosDePagamento_Collection()
    {
        // Arrange & Act
        var responsavel = TestDataBuilder.CreateValidResponsavel();

        // Assert
        responsavel.PlanosDePagamento.Should().NotBeNull();
        responsavel.PlanosDePagamento.Should().BeEmpty();
    }

    [Fact]
    public void ResponsavelFinanceiro_Should_Accept_Valid_Nome()
    {
        // Arrange
        var responsavel = new ResponsavelFinanceiro { Nome = "Maria Silva" };

        // Act & Assert
        responsavel.Nome.Should().Be("Maria Silva");
    }

    [Fact]
    public void ResponsavelFinanceiro_Should_Initialize_With_Empty_Collection()
    {
        // Arrange & Act
        var responsavel = new ResponsavelFinanceiro { Nome = "Test" };

        // Assert
        responsavel.PlanosDePagamento.Should().NotBeNull();
        responsavel.PlanosDePagamento.Should().BeOfType<List<PlanoDePagamento>>();
    }
}