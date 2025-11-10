using Moq;
using SoftCep.Application.Services;
using SoftCep.Domain.Entities;
using SoftCep.Domain.Interfaces;

namespace SoftCep.Tests;

public class CepServiceTests
{
    [Fact]
    public async Task GetByCepAsync_ReturnsDto_FromLocalDatabase_WhenExists()
    {
        // arrange
        var gatewayMock = new Mock<IViaCepGateway>();
        var repoMock = new Mock<IEnderecoRepository>();

        var endereco = new Endereco
        {
            Id = "b7a8d8e1-1b4e-4c33-a0ce-8d40a5f73b21",
            Cep = "01001000",
            Logradouro = "Praça da Sé",
            Bairro = "Sé",
            Cidade = "São Paulo",
            Uf = "SP"
        };


        repoMock.Setup(r => r.GetByCepAsync("01001000", It.IsAny<CancellationToken>()))
                .ReturnsAsync(endereco);

        var svc = new CepService(gatewayMock.Object, repoMock.Object);

        // act
        var result = await svc.GetByCepAsync("01001000");

        // assert
        Assert.NotNull(result);
        Assert.Equal("01001000", result!.Cep);
        Assert.Equal("São Paulo", result.Localidade);
        
    }

    [Fact]
    public async Task GetByCepAsync_ReturnsDto_FromViaCep_WhenNotInLocalDatabase()
    {
        // arrange
        var gatewayMock = new Mock<IViaCepGateway>();
        var repoMock = new Mock<IEnderecoRepository>();

        repoMock.Setup(r => r.GetByCepAsync("01001000", It.IsAny<CancellationToken>()))
                .ReturnsAsync((Endereco?)null);

        var domainCep = new Cep("01001000");
        domainCep.Populate("Praça da Sé", "Sé", "São Paulo", "SP");

        gatewayMock.Setup(g => g.GetByCepAsync("01001000", It.IsAny<CancellationToken>()))
                   .ReturnsAsync(domainCep);

        var svc = new CepService(gatewayMock.Object, repoMock.Object);

        // act
        var result = await svc.GetByCepAsync("01001000");

        // assert
        Assert.NotNull(result);
        Assert.Equal("01001000", result!.Cep);
        Assert.Equal("São Paulo", result.Localidade);
    }

    [Fact]
    public async Task GetByCepAsync_ReturnsNull_WhenNotFoundAnywhere()
    {
        // arrange
        var gatewayMock = new Mock<IViaCepGateway>();
        var repoMock = new Mock<IEnderecoRepository>();

        // buscar na api
        gatewayMock.Setup(g => g.GetByCepAsync("00000000", It.IsAny<CancellationToken>()))
                   .ReturnsAsync((Cep?)null);

        // buscar na base de dados
        repoMock.Setup(r => r.GetByCepAsync("00000000", It.IsAny<CancellationToken>()))
                .ReturnsAsync((Endereco?)null);

        var svc = new CepService(gatewayMock.Object, repoMock.Object);

        // act
        var result = await svc.GetByCepAsync("00000000");

        // assert
        Assert.Null(result);
    }
}
