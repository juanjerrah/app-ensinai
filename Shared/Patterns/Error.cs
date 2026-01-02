namespace app_ensinai.Shared.Patterns;

/// <summary>
/// Representa um erro no Result Pattern
/// </summary>
public class Error
{
    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }

    public Error(string code, string message, ErrorType type = ErrorType.Failure)
    {
        Code = code;
        Message = message;
        Type = type;
    }

    public static Error None => new(string.Empty, string.Empty, ErrorType.None);
    public static Error NullValue => new("Error.NullValue", "O valor especificado é nulo", ErrorType.Failure);
    
    public static Error NotFound(string message) => new("Error.NotFound", message, ErrorType.NotFound);
    public static Error Validation(string message) => new("Error.Validation", message, ErrorType.Validation);
    public static Error Unauthorized(string message) => new("Error.Unauthorized", message, ErrorType.Unauthorized);
    public static Error Conflict(string message) => new("Error.Conflict", message, ErrorType.Conflict);
    public static Error Failure(string message) => new("Error.Failure", message, ErrorType.Failure);
}

/// <summary>
/// Tipos de erro
/// </summary>
public enum ErrorType
{
    None = 0,
    Failure = 1,
    Validation = 2,
    NotFound = 3,
    Unauthorized = 4,
    Conflict = 5
}
