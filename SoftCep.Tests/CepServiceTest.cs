using Moq;
using SoftCep.Application.Services;
using SoftCep.Domain.Entities;
using SoftCep.Domain.Interfaces;

namespace SoftCep.Tests;

public class CepServiceTests
{
    [Fact]
    public async Task GetByCepAsync_ReturnsDto_WhenGatewayReturnsDomain()
    {
        // arrange
        var gatewayMock = new Mock<IViaCepGateway>();
        var domainCep = new Cep("01001000");
        domainCep.Populate("Praça da Sé", "Sé", "São Paulo", "SP");

        gatewayMock.Setup(g => g.GetByCepAsync("01001000", It.IsAny<CancellationToken>()))
                   .ReturnsAsync(domainCep);

        var svc = new CepService(gatewayMock.Object);

        // act
        var result = await svc.GetByCepAsync("01001000");

        // assert
        Assert.NotNull(result);
        Assert.Equal("01001000", result!.Cep);
        Assert.Equal("São Paulo", result.Localidade);
    }

    [Fact]
    public async Task GetByCepAsync_ReturnsNull_WhenNotFound()
    {
        var gatewayMock = new Mock<IViaCepGateway>();
        gatewayMock.Setup(g => g.GetByCepAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync((Cep?)null);

        var svc = new CepService(gatewayMock.Object);

        var result = await svc.GetByCepAsync("00000000");
        Assert.Null(result);
    }
}
