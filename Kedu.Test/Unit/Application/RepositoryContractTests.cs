using FluentAssertions;
using Kedu.Application.Interfaces;
using Kedu.Domain.Entities;
using Kedu.Test.Fixtures;
using Moq;
using Xunit;

namespace Kedu.Test.Unit.Application;

public class ResponsavelFinanceiroRepositoryContractTests
{
    private readonly Mock<IResponsavelFinanceiroRepository> _mockRepository;

    public ResponsavelFinanceiroRepositoryContractTests()
    {
        _mockRepository = new Mock<IResponsavelFinanceiroRepository>();
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Entity_When_Setup_Correctly()
    {
        // Arrange
        var expectedResponsavel = TestDataBuilder.CreateValidResponsavel();
        _mockRepository.Setup(x => x.GetByIdAsync(1))
                      .ReturnsAsync(expectedResponsavel);

        // Act
        var result = await _mockRepository.Object.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(expectedResponsavel);
        _mockRepository.Verify(x => x.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetWithPlanosAsync_Should_Call_Correct_Method()
    {
        // Arrange
        var responsavel = TestDataBuilder.CreateValidResponsavel();
        responsavel.PlanosDePagamento = new List<PlanoDePagamento>();
        
        _mockRepository.Setup(x => x.GetWithPlanosAsync(1))
                      .ReturnsAsync(responsavel);

        // Act
        var result = await _mockRepository.Object.GetWithPlanosAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.PlanosDePagamento.Should().NotBeNull();
        _mockRepository.Verify(x => x.GetWithPlanosAsync(1), Times.Once);
    }

    [Fact]
    public async Task AddAsync_Should_Return_Added_Entity()
    {
        // Arrange
        var newResponsavel = TestDataBuilder.CreateValidResponsavel();
        newResponsavel.Id = 0;
        
        _mockRepository.Setup(x => x.AddAsync(It.IsAny<ResponsavelFinanceiro>()))
                      .ReturnsAsync((ResponsavelFinanceiro r) => { r.Id = 1; return r; });

        // Act
        var result = await _mockRepository.Object.AddAsync(newResponsavel);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        _mockRepository.Verify(x => x.AddAsync(It.IsAny<ResponsavelFinanceiro>()), Times.Once);
    }
}