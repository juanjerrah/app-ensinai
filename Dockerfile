# ==============================================================================
# Multi-stage Dockerfile para aplicação .NET 10
# Otimizado para segurança e tamanho de imagem reduzido
# ==============================================================================

# ------------------------------------------------------------------------------
# Stage 1: Build
# Usa SDK completo para compilar a aplicação
# ------------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

# Copiar apenas os arquivos de projeto primeiro (melhor cache de layers)
COPY ["app-ensinai.csproj", "./"]

# Restaurar dependências em layer separado (cache otimizado)
RUN dotnet restore "app-ensinai.csproj"

# Copiar todo o código fonte
COPY . .

# Build da aplicação em modo Release
RUN dotnet build "app-ensinai.csproj" \
    -c Release \
    -o /app/build

# ------------------------------------------------------------------------------
# Stage 2: Publish
# Publica a aplicação otimizada
# ------------------------------------------------------------------------------
FROM build AS publish
RUN dotnet publish "app-ensinai.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# ------------------------------------------------------------------------------
# Stage 3: Runtime
# Imagem final leve e segura apenas com runtime
# ------------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final

# Metadados da imagem
LABEL maintainer="EnsinAI Team <contato@ensinai.com>" \
      version="1.0" \
      description="EnsinAI API - Sistema de agendamento de aulas"

# Criar usuário não-root para executar a aplicação (segurança)
RUN addgroup -g 1000 appuser && \
    adduser -u 1000 -G appuser -s /bin/sh -D appuser

# Definir diretório de trabalho
WORKDIR /app

# Copiar arquivos publicados do stage anterior
COPY --from=publish --chown=appuser:appuser /app/publish .

# Configurar timezone (ajuste conforme necessário)
ENV TZ=America/Sao_Paulo

# Configurações de segurança e otimização
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    DOTNET_RUNNING_IN_CONTAINER=true \
    ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    # Desabilitar telemetria da Microsoft
    DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    # Otimizações de runtime
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    DOTNET_EnableDiagnostics=0

# Criar diretório para uploads/temp com permissões corretas
RUN mkdir -p /app/temp && \
    chown -R appuser:appuser /app/temp

# Expor porta não privilegiada (>1024 para segurança)
EXPOSE 8080

# Mudar para usuário não-root
USER appuser

# Health check para monitoramento
HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
    CMD wget --no-verbose --tries=1 --spider http://localhost:8080/health || exit 1

# Executar a aplicação
ENTRYPOINT ["dotnet", "app-ensinai.dll"]
