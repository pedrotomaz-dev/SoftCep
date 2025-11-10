# --- Build stage ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia apenas arquivos de projeto para aproveitar cache de camadas
COPY ["SoftCep.sln", "./"]
COPY ["SoftCep.Api/SoftCep.Api.csproj", "SoftCep.Api/"]
COPY ["SoftCep.Application/SoftCep.Application.csproj", "SoftCep.Application/"]
COPY ["SoftCep.Domain/SoftCep.Domain.csproj", "SoftCep.Domain/"]
COPY ["SoftCep.Infra/SoftCep.Infra.csproj", "SoftCep.Infra/"]

# Restaura dependências (cacheável)
RUN dotnet restore "SoftCep.Api/SoftCep.Api.csproj"

# Copia todo o restante do código
COPY . .

# Publica (output em /app/publish)
WORKDIR /src/SoftCep.Api
RUN dotnet publish "SoftCep.Api.csproj" -c Release -o /app/publish --no-restore

# --- Runtime stage ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Define a porta que a aplicação irá escutar internamente
ENV ASPNETCORE_URLS=http://+:8080

# Copia os artefatos publicados
COPY --from=build /app/publish .

# Expõe a porta padrão
EXPOSE 8080

# Healthcheck baseado no endpoint /health
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "SoftCep.Api.dll"]

