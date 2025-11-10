using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using SoftCep.Domain.Interfaces;
using SoftCep.Infrastructure.Http;

namespace SoftCep.Infra.Extensions;

public static class HttpClientExtensions
{
    public static IServiceCollection AddViaCepClient(this IServiceCollection services)
    {
        services.AddHttpClient<IViaCepGateway, ViaCepGateway>(client =>
        {
            client.BaseAddress = new Uri("https://viacep.com.br/");
            client.Timeout = TimeSpan.FromSeconds(10);
        })
        .AddPolicyHandler(GetRetryPolicy())
        .AddPolicyHandler(GetCircuitBreakerPolicy());

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
}
