using SoftCep.Domain.Entities;

namespace SoftCep.Application.Interfaces;

public interface ICepCacheService
{
    Task<Cep?> GetAsync(string cep);
    Task SetAsync(Cep cep, TimeSpan ttl);
}
