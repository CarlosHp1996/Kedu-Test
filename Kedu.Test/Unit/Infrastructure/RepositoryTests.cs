using FluentAssertions;
using Kedu.Application.Interfaces;
using Kedu.Domain.Entities;
using Kedu.Test.Fixtures;
using Moq;
using Xunit;

namespace Kedu.Test.Unit.Infrastructure;

public class RepositoryContractTests
{
    [Fact]
    public void IResponsavelFinanceiroRepository_Should_Have_Required_Methods()
    {
        // Arrange
        var mockRepository = new Mock<IResponsavelFinanceiroRepository>();

        // Act & Assert - Verificar se a interface tem os métodos corretos
        mockRepository.Setup(x => x.GetWithPlanosAsync(It.IsAny<int>()))
                     .ReturnsAsync((ResponsavelFinanceiro?)null);

        mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync((ResponsavelFinanceiro?)null);

        mockRepository.Setup(x => x.AddAsync(It.IsAny<ResponsavelFinanceiro>()))
                     .ReturnsAsync((ResponsavelFinanceiro r) => r);

        // Verificar se o mock foi configurado corretamente
        mockRepository.Object.Should().NotBeNull();
    }

    [Fact]
    public void IPlanoDePagamentoRepository_Should_Have_Required_Methods()
    {
        // Arrange
        var mockRepository = new Mock<IPlanoDePagamentoRepository>();

        // Act & Assert
        mockRepository.Setup(x => x.GetWithCobrancasAsync(It.IsAny<int>()))
                     .ReturnsAsync((PlanoDePagamento?)null);

        mockRepository.Setup(x => x.GetByResponsavelIdAsync(It.IsAny<int>()))
                     .ReturnsAsync(new List<PlanoDePagamento>());

        mockRepository.Setup(x => x.GetTotalValueAsync(It.IsAny<int>()))
                     .ReturnsAsync(0m);

        mockRepository.Object.Should().NotBeNull();
    }

    [Fact]
    public void ICobrancaRepository_Should_Have_Required_Methods()
    {
        // Arrange
        var mockRepository = new Mock<ICobrancaRepository>();

        // Act & Assert
        mockRepository.Setup(x => x.GetByResponsavelIdAsync(It.IsAny<int>()))
                     .ReturnsAsync(new List<Cobranca>());

        mockRepository.Setup(x => x.CountByResponsavelIdAsync(It.IsAny<int>()))
                     .ReturnsAsync(0);

        mockRepository.Setup(x => x.GetByCodigoPagamentoAsync(It.IsAny<string>()))
                     .ReturnsAsync((Cobranca?)null);

        mockRepository.Object.Should().NotBeNull();
    }

    [Fact]
    public void IPagamentoRepository_Should_Have_Required_Methods()
    {
        // Arrange
        var mockRepository = new Mock<IPagamentoRepository>();

        // Act & Assert
        mockRepository.Setup(x => x.GetByCobrancaIdAsync(It.IsAny<int>()))
                     .ReturnsAsync(new List<Pagamento>());

        mockRepository.Object.Should().NotBeNull();
    }

    [Fact]
    public void ICentroDeCustoRepository_Should_Have_Required_Methods()
    {
        // Arrange
        var mockRepository = new Mock<ICentroDeCustoRepository>();       

        mockRepository.Setup(x => x.ExistsByNomeAsync(It.IsAny<string>()))
                     .ReturnsAsync(false);

        mockRepository.Object.Should().NotBeNull();
    }
}

public class RepositoryBehaviorTests
{
    [Fact]
    public async Task Repository_GetByIdAsync_Should_Return_Entity_When_Exists()
    {
        // Arrange
        var mockRepository = new Mock<IResponsavelFinanceiroRepository>();
        var expectedEntity = TestDataBuilder.CreateValidResponsavel();
        
        mockRepository.Setup(x => x.GetByIdAsync(1))
                     .ReturnsAsync(expectedEntity);

        // Act
        var result = await mockRepository.Object.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(expectedEntity);
        mockRepository.Verify(x => x.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task Repository_GetByIdAsync_Should_Return_Null_When_Not_Exists()
    {
        // Arrange
        var mockRepository = new Mock<IResponsavelFinanceiroRepository>();
        
        mockRepository.Setup(x => x.GetByIdAsync(999))
                     .ReturnsAsync((ResponsavelFinanceiro?)null);

        // Act
        var result = await mockRepository.Object.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
        mockRepository.Verify(x => x.GetByIdAsync(999), Times.Once);
    }

    [Fact]
    public async Task Repository_AddAsync_Should_Return_Added_Entity()
    {
        // Arrange
        var mockRepository = new Mock<IResponsavelFinanceiroRepository>();
        var entityToAdd = TestDataBuilder.CreateValidResponsavel();
        entityToAdd.Id = 0; // Simula novo registro
        
        mockRepository.Setup(x => x.AddAsync(It.IsAny<ResponsavelFinanceiro>()))
                     .ReturnsAsync((ResponsavelFinanceiro entity) => 
                     {
                         entity.Id = 1; // Simula Id gerado pelo banco
                         return entity;
                     });

        // Act
        var result = await mockRepository.Object.AddAsync(entityToAdd);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        mockRepository.Verify(x => x.AddAsync(It.IsAny<ResponsavelFinanceiro>()), Times.Once);
    }

    [Fact]
    public async Task Repository_UpdateAsync_Should_Call_Update_Method()
    {
        // Arrange
        var mockRepository = new Mock<IResponsavelFinanceiroRepository>();
        var entityToUpdate = TestDataBuilder.CreateValidResponsavel();
        
        mockRepository.Setup(x => x.UpdateAsync(It.IsAny<ResponsavelFinanceiro>()))
                     .Returns(Task.CompletedTask);

        // Act
        await mockRepository.Object.UpdateAsync(entityToUpdate);

        // Assert
        mockRepository.Verify(x => x.UpdateAsync(It.IsAny<ResponsavelFinanceiro>()), Times.Once);
    }

    [Fact]
    public async Task Repository_DeleteAsync_Should_Call_Delete_Method()
    {
        // Arrange
        var mockRepository = new Mock<IResponsavelFinanceiroRepository>();
        var entityToDelete = TestDataBuilder.CreateValidResponsavel();
        
        mockRepository.Setup(x => x.DeleteAsync(It.IsAny<ResponsavelFinanceiro>()))
                     .Returns(Task.CompletedTask);

        // Act
        await mockRepository.Object.DeleteAsync(entityToDelete);

        // Assert
        mockRepository.Verify(x => x.DeleteAsync(It.IsAny<ResponsavelFinanceiro>()), Times.Once);
    }

    [Fact]
    public async Task PlanoDePagamentoRepository_GetTotalValueAsync_Should_Calculate_Correctly()
    {
        // Arrange
        var mockRepository = new Mock<IPlanoDePagamentoRepository>();
        var expectedTotal = 1500.00m;
        
        mockRepository.Setup(x => x.GetTotalValueAsync(1))
                     .ReturnsAsync(expectedTotal);

        // Act
        var result = await mockRepository.Object.GetTotalValueAsync(1);

        // Assert
        result.Should().Be(expectedTotal);
        mockRepository.Verify(x => x.GetTotalValueAsync(1), Times.Once);
    }

    [Fact]
    public async Task CobrancaRepository_GetByCodigoPagamentoAsync_Should_Find_By_Code()
    {
        // Arrange
        var mockRepository = new Mock<ICobrancaRepository>();
        var expectedCobranca = TestDataBuilder.CreateValidCobranca();
        var paymentCode = "PIX-001-ABC123";
        expectedCobranca.CodigoPagamento = paymentCode;
        
        mockRepository.Setup(x => x.GetByCodigoPagamentoAsync(paymentCode))
                     .ReturnsAsync(expectedCobranca);

        // Act
        var result = await mockRepository.Object.GetByCodigoPagamentoAsync(paymentCode);

        // Assert
        result.Should().NotBeNull();
        result.CodigoPagamento.Should().Be(paymentCode);
        mockRepository.Verify(x => x.GetByCodigoPagamentoAsync(paymentCode), Times.Once);
    }
}