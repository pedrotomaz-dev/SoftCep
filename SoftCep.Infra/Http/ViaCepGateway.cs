using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using SoftCep.Domain.Entities;
using SoftCep.Domain.Exceptions;
using SoftCep.Domain.Interfaces;

namespace SoftCep.Infrastructure.Http;

internal record ViaCepResponse(
    string? cep,
    string? logradouro,
    string? complemento,
    string? bairro,
    string? localidade,
    string? uf,
    string? ibge,
    string? gia,
    string? ddd,
    string? siafi,
    bool? erro = null);

public class ViaCepGateway : IViaCepGateway
{
    private readonly HttpClient _http;
    private readonly ILogger<ViaCepGateway> _logger;

    public ViaCepGateway(HttpClient http, ILogger<ViaCepGateway> logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task<Cep?> GetByCepAsync(string cep, CancellationToken cancellationToken = default)
    {
        var onlyDigits = new string(cep.Where(char.IsDigit).ToArray());
        if (onlyDigits.Length != 8)
        {
            _logger.LogWarning("CEP inválido informado: {Cep}", cep);
            return null;
        }

        var url = $"ws/{onlyDigits}/json/";

        try
        {
            _logger.LogInformation("Consultando ViaCEP pelo CEP {Cep}", cep);

            var res = await _http.GetFromJsonAsync<ViaCepResponse>(url, cancellationToken);

            if (res is null)
            {
                _logger.LogWarning("ViaCEP retornou nulo para o CEP {Cep}", cep);
                return null;
            }

            if (res.erro == true)
            {
                _logger.LogWarning("ViaCEP retornou erro para o CEP {Cep}", cep);
                return null;
            }

            var domain = new Cep(onlyDigits);
            domain.Populate(res.logradouro, res.bairro, res.localidade, res.uf);

            return domain;
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout ao consultar ViaCEP para o CEP {Cep}", cep);
            throw new ExternalServiceTimeoutException("Timeout ao consultar o serviço ViaCEP.", ex);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Falha de comunicação com o ViaCEP para o CEP {Cep}", cep);
            throw new ExternalServiceUnavailableException("Falha de comunicação com o ViaCEP.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao consultar ViaCEP para o CEP {Cep}", cep);
            throw new ExternalServiceException("Erro inesperado ao consultar o serviço ViaCEP.", ex);
        }
    }

    public async Task<IEnumerable<Cep>> GetByAddressAsync(string uf, string cidade, string logradouro, CancellationToken cancellationToken = default)
    {
        var url = $"ws/{uf}/{Uri.EscapeDataString(cidade)}/{Uri.EscapeDataString(logradouro)}/json/";

        try
        {
            _logger.LogInformation("Consultando ViaCEP por endereço: {Uf}/{Cidade}/{Logradouro}", uf, cidade, logradouro);

            var results = await _http.GetFromJsonAsync<List<ViaCepResponse>>(url, cancellationToken);

            if (results is null || results.Count == 0)
            {
                _logger.LogWarning("ViaCEP retornou nenhum resultado para o endereço informado.");
                return Enumerable.Empty<Cep>();
            }

            return results
                .Where(r => r.erro != true)
                .Select(r =>
                {
                    var cepDigits = r.cep is null ? string.Empty : new string(r.cep.Where(char.IsDigit).ToArray());
                    var cep = new Cep(cepDigits);
                    cep.Populate(r.logradouro, r.bairro, r.localidade, r.uf);
                    return cep;
                });
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout ao consultar ViaCEP por endereço {Uf}/{Cidade}/{Logradouro}", uf, cidade, logradouro);
            throw new ExternalServiceTimeoutException("Timeout ao consultar o serviço ViaCEP.", ex);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Falha de comunicação com o ViaCEP por endereço {Uf}/{Cidade}/{Logradouro}", uf, cidade, logradouro);
            throw new ExternalServiceUnavailableException("Falha de comunicação com o ViaCEP.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao consultar ViaCEP por endereço {Uf}/{Cidade}/{Logradouro}", uf, cidade, logradouro);
            throw new ExternalServiceException("Erro inesperado ao consultar o serviço ViaCEP.", ex);
        }
    }
}
