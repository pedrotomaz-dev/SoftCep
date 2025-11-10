using Microsoft.EntityFrameworkCore;
using SoftCep.Domain.Entities;
using SoftCep.Domain.Interfaces;
using SoftCep.Infra.Database;

namespace SoftCep.Infra.Repositories;

public class EnderecoRepository(SoftCepContext context) : IEnderecoRepository
{
    public async Task<Endereco?> GetByCepAsync(string cep, CancellationToken cancellationToken)
    {
        var onlyDigits = new string(cep.Where(char.IsDigit).ToArray());
        return await context.Enderecos.FirstOrDefaultAsync(e => e.Cep == onlyDigits);
    }
}
