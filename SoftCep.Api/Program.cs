using Microsoft.EntityFrameworkCore;
using SoftCep.Api.Endpoints;
using SoftCep.Api.Extensions;
using SoftCep.Application.Extensions;
using SoftCep.Infra.Data;
using SoftCep.Infra.Database;
using SoftCep.Infra.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Caminho do banco SQLite dentro da pasta da aplicação
var dbPath = Path.Combine(AppContext.BaseDirectory, "softcep.db");

// Registrar o contexto
builder.Services.AddDbContext<SoftCepContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// Caminho do arquivo de seed
//var seedPath = Path.Combine(AppContext.BaseDirectory, "..", "SoftCep.Infra", "Data", "seed_ceps.sql");
var seedPath = Path.Combine(AppContext.BaseDirectory, "Data", "seed_ceps.sql");
//var seedPath = builder.Configuration["Database:SeedFile"];


// Inicializa o banco e popula dados se necessário
DbInitializer.Initialize(dbPath, seedPath);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");


app.UseGlobalExceptionHandling(app.Logger);

//if (app.Environment.IsDevelopment()) 
//{ 
    app.UseSwagger(); 
    app.UseSwaggerUI(); 
//}

app.MapCepEndpoints();

app.Run();
