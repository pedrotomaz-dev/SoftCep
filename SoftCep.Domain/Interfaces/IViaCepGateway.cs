using SoftCep.Domain.Entities;

namespace SoftCep.Domain.Interfaces;

public interface IViaCepGateway
{
    /// <summary>
    /// Consulta o ViaCEP por CEP (somente dígitos).
    /// Retorna null se não encontrado.
    /// </summary>
    Task<Cep?> GetByCepAsync(string cep, CancellationToken cancellationToken = default);

    /// <summary>
    /// Consulta por endereço (uf, cidade, logradouro) — pode retornar múltiplos resultados;
    /// devolvemos o primeiro (camada application decide política).
    /// </summary>
    Task<IEnumerable<Cep>> GetByAddressAsync(string uf, string cidade, string logradouro, CancellationToken cancellationToken = default);
}
