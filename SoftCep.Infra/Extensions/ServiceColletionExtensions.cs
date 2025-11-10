using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoftCep.Domain.Interfaces;
using SoftCep.Infra.Repositories;

namespace SoftCep.Infra.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddViaCepClient();

        services.AddScoped<IEnderecoRepository, EnderecoRepository>();

        return services;
    }
}
