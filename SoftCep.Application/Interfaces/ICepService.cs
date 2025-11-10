using SoftCep.Application.DTOs;

namespace SoftCep.Application.Interfaces;
public interface ICepService
{
    Task<CepDto?> GetByCepAsync(string cep, CancellationToken cancellationToken = default);
    Task<IEnumerable<CepDto>> GetByAddressAsync(string uf, string cidade, string logradouro, CancellationToken cancellationToken = default);
}