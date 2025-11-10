using Microsoft.EntityFrameworkCore;
using SoftCep.Domain.Entities;

namespace SoftCep.Infra.Database;

public class SoftCepContext : DbContext
{
    public SoftCepContext(DbContextOptions<SoftCepContext> options) : base(options) { }

    public DbSet<Endereco> Enderecos { get; set; }
}
