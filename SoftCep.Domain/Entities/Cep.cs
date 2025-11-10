namespace SoftCep.Domain.Entities;

public sealed class Cep
{
    public string Numero { get; }
    public string? Logradouro { get; private set; }
    public string? Bairro { get; private set; }
    public string? Localidade { get; private set; }
    public string? Uf { get; private set; }

    public Cep(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("CEP inválido", nameof(value));

        // normalize: remove non-digits
        Numero = new string(value.Where(char.IsDigit).ToArray());
        if (Numero.Length != 8)
            throw new ArgumentException("CEP deve conter 8 dígitos.", nameof(value));
    }

    public void Populate(string? logradouro, string? bairro, string? localidade, string? uf)
    {
        Logradouro = logradouro;
        Bairro = bairro;
        Localidade = localidade;
        Uf = uf;
    }
}
