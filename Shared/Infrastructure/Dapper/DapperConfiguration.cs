using Dapper;
using System.Reflection;

namespace app_ensinai.Shared.Infrastructure.Dapper;

/// <summary>
/// Configuração global do Dapper para mapeamento automático
/// </summary>
public static class DapperConfiguration
{
    private static bool _isConfigured = false;
    private static readonly object _lock = new();

    /// <summary>
    /// Configura o Dapper com mapeamentos customizados
    /// </summary>
    public static void Configure()
    {
        if (_isConfigured) return;

        lock (_lock)
        {
            if (_isConfigured) return;

            // Configura o mapeador de snake_case para PascalCase
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            _isConfigured = true;
        }
    }

    /// <summary>
    /// Registra um tipo específico com mapeamento customizado
    /// </summary>
    public static void RegisterTypeMap<T>()
    {
        SqlMapper.SetTypeMap(
            typeof(T),
            new CustomPropertyTypeMap(
                typeof(T),
                (type, columnName) => 
                {
                    var property = type.GetProperty(
                        ToPascalCase(columnName),
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase
                    );
                    return property;
                }
            )
        );
    }

    /// <summary>
    /// Converte snake_case para PascalCase
    /// </summary>
    private static string ToPascalCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // Remove caracteres especiais e divide por underscore
        var parts = input.Split('_', StringSplitOptions.RemoveEmptyEntries);
        
        return string.Concat(parts.Select(part => 
            char.ToUpperInvariant(part[0]) + part.Substring(1).ToLowerInvariant()
        ));
    }
}
