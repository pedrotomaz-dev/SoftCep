using SoftCep.Api.Endpoints;
using SoftCep.Api.Extensions;
using SoftCep.Application.Extensions;
using SoftCep.Infra.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseGlobalExceptionHandling(app.Logger);

if (app.Environment.IsDevelopment()) 
{ 
    app.UseSwagger(); 
    app.UseSwaggerUI(); 
}

app.MapCepEndpoints();

app.Run();
