namespace SoftCep.Application.DTOs;

public record CepDto(string Cep, string? Logradouro, string? Bairro, string? Localidade, string? Uf);
