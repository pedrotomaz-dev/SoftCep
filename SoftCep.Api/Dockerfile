# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia os arquivos de projeto e restaura dependências
COPY *.sln ./
COPY SoftCep.Api/SoftCep.Api.csproj SoftCep.Api/
COPY SoftCep.Application/SoftCep.Application.csproj SoftCep.Application/
COPY SoftCep.Domain/SoftCep.Domain.csproj SoftCep.Domain/
COPY SoftCep.Infra/SoftCep.Infra.csproj SoftCep.Infra/
RUN dotnet restore SoftCep.Api/SoftCep.Api.csproj

# Copia o restante do código e compila
COPY . .
WORKDIR /app/SoftCep.Api
RUN dotnet publish -c Release -o /out

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /out .

# Expõe a porta padrão
EXPOSE 8080
ENTRYPOINT ["dotnet", "SoftCep.Api.dll"]
