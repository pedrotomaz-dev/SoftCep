using SoftCep.Application.Interfaces;

namespace SoftCep.Api.Endpoints;

public static class CepEndpoints
{
    public static void MapCepEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/cep").WithTags("CEP");

        // GET /api/cep/{cep}
        group.MapGet("/{cep}", async (string cep, ICepService service, CancellationToken ct) =>
        {
            var result = await service.GetByCepAsync(cep, ct);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        })
        .WithSummary("Consulta um CEP específico.")
        .WithOpenApi();

        // GET /api/cep/endereco
        group.MapGet("/endereco", async (string uf, string cidade, string logradouro, ICepService service, CancellationToken ct) =>
        {
            var result = await service.GetByAddressAsync(uf, cidade, logradouro, ct);
            return result.Any() ? Results.Ok(result) : Results.NotFound();
        })
        .WithSummary("Consulta CEPs por UF, cidade e logradouro.")
        .WithOpenApi();
    }
}
