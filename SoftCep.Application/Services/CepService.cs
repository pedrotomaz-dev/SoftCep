using SoftCep.Application.DTOs;
using SoftCep.Application.Interfaces;
using SoftCep.Domain.Entities;
using SoftCep.Domain.Interfaces;

namespace SoftCep.Application.Services;
public class CepService : ICepService
{
    private readonly IViaCepGateway _viaCep;
    public CepService(IViaCepGateway viaCep) => _viaCep = viaCep;

    public async Task<CepDto?> GetByCepAsync(string cep, CancellationToken cancellationToken = default)
    {
        var domain = await _viaCep.GetByCepAsync(cep, cancellationToken);
        if (domain is null) return null;
        return ToDto(domain);   
    }

    public async Task<IEnumerable<CepDto>> GetByAddressAsync(string uf, string cidade, string logradouro, CancellationToken cancellationToken = default)
    {
        var list = await _viaCep.GetByAddressAsync(uf, cidade, logradouro, cancellationToken);
        return list.Select(ToDto);
    }

    private static CepDto ToDto(Cep c) => new CepDto(c.Value, c.Logradouro, c.Bairro, c.Localidade, c.Uf);
}