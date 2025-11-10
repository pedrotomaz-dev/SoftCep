using Microsoft.Extensions.Caching.Memory;
using SoftCep.Application.Interfaces;
using SoftCep.Domain.Entities;

namespace SoftCep.Application.Services;

public class CepCacheService : ICepCacheService
{
    private readonly IMemoryCache _cache;
    public CepCacheService(IMemoryCache cache) => _cache = cache;

    public Task<Cep?> GetAsync(string cep)
    {
        _cache.TryGetValue(cep, out Cep? value);
        return Task.FromResult(value);
    }

    public Task SetAsync(Cep cep, TimeSpan ttl)
    {
        _cache.Set(cep.Numero, cep, ttl);
        return Task.CompletedTask;
    }
}
