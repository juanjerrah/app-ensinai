using Npgsql;
using System.Data;

namespace app_ensinai.Shared.CrossCutting;

public static class PostgresSetup
{
    /// <summary>
    /// Configura a conexão com PostgreSQL
    /// </summary>
    public static IServiceCollection AddPostgresConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var host = configuration["PG_HOST"] 
            ?? throw new ArgumentNullException("PG_HOST", "Variável de ambiente PG_HOST não configurada no launchSettings.json");
        
        var port = configuration["PG_PORT"] 
            ?? throw new ArgumentNullException("PG_PORT", "Variável de ambiente PG_PORT não configurada no launchSettings.json");
        
        var database = configuration["PG_DATABASE"] 
            ?? throw new ArgumentNullException("PG_DATABASE", "Variável de ambiente PG_DATABASE não configurada no launchSettings.json");
        
        var username = configuration["PG_USER"] 
            ?? throw new ArgumentNullException("PG_USER", "Variável de ambiente PG_USER não configurada no launchSettings.json");
        
        var password = configuration["PG_PASSWORD"] 
            ?? throw new ArgumentNullException("PG_PASSWORD", "Variável de ambiente PG_PASSWORD não configurada no launchSettings.json");

        var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";

        // Valida a connection string
        ValidateConnectionString(connectionString);

        // Registra a connection string como singleton
        services.AddSingleton(connectionString);

        // Registra IDbConnection como scoped (uma nova conexão por request)
        services.AddScoped<IDbConnection>(sp => 
        {
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            return connection;
        });

        Console.WriteLine($"✅ PostgreSQL configurado com sucesso! ({host}:{port}/{database})");

        return services;
    }

    /// <summary>
    /// Valida se a connection string está corretamente formatada
    /// </summary>
    private static void ValidateConnectionString(string connectionString)
    {
        try
        {
            var builder = new NpgsqlConnectionStringBuilder(connectionString);
            
            if (string.IsNullOrEmpty(builder.Host))
                throw new ArgumentException("Host não especificado na connection string");
            
            if (string.IsNullOrEmpty(builder.Database))
                throw new ArgumentException("Database não especificado na connection string");
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Connection string inválida: {ex.Message}", ex);
        }
    }
}
