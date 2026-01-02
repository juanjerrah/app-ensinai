using Npgsql;

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
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException("ConnectionStrings:DefaultConnection", 
                "Connection string do PostgreSQL não configurada");

        var connectionTest = new NpgsqlConnection(connectionString);
        connectionTest.Open();
        connectionTest.Close();

        services.AddTransient(delegate (IServiceProvider sp)
        {
            var connection = new NpgsqlConnection(connectionString);
            return connection;
        });

        return services;
    }

    
}
