using Microsoft.Extensions.DependencyInjection;
using SoftCep.Application.Services;
using SoftCep.Application.Interfaces;

namespace SoftCep.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMemoryCache();

        services.AddScoped<ICepService, CepService>();
        services.AddScoped<ICepCacheService, CepCacheService>();
        
        return services;
    }
}
