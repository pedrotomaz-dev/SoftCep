using SoftCep.Domain.Entities;

namespace SoftCep.Domain.Interfaces;

public interface IEnderecoRepository
{
    Task<Endereco?> GetByCepAsync(string cep, CancellationToken cancellationToken);
}
