using SoftCep.Application.DTOs;
using SoftCep.Application.Interfaces;
using SoftCep.Domain.Entities;
using SoftCep.Domain.Interfaces;

namespace SoftCep.Application.Services;

public class CepService(IViaCepGateway viaCep) : ICepService
{
    public async Task<CepDto?> GetByCepAsync(string cep, CancellationToken cancellationToken = default)
    {
        Validate(cep);

        var domain = await viaCep.GetByCepAsync(cep, cancellationToken);
        if (domain is null) return null;
        return ToDto(domain);   
    }

    
    public async Task<IEnumerable<CepDto>> GetByAddressAsync(string uf, string cidade, string logradouro, CancellationToken cancellationToken = default)
    {
        var list = await viaCep.GetByAddressAsync(uf, cidade, logradouro, cancellationToken);
        return list.Select(ToDto);
    }


    #region Private Methods
    private CepDto ToDto(Cep c) => new CepDto(c.Value, c.Logradouro, c.Bairro, c.Localidade, c.Uf);


    private void Validate(string cep)
    {
        string somenteDigitos = new string(cep.Where(char.IsDigit).ToArray());
        
        if (string.IsNullOrWhiteSpace(somenteDigitos))
            throw new ArgumentException("CEP inválido", nameof(cep));

        if (somenteDigitos.Length != 8 || !somenteDigitos.All(char.IsDigit))
            throw new ArgumentException("CEP deve conter 8 dígitos.", nameof(cep));
    }
    #endregion
}