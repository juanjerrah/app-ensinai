namespace app_ensinai.Shared.Patterns;

/// <summary>
/// Result Pattern para operações que retornam valor
/// </summary>
public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public Error Error { get; }

    private Result(bool isSuccess, T? value, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException("Erro inválido para um resultado de sucesso");

        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException("Erro obrigatório para um resultado de falha");

        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    /// <summary>
    /// Cria um resultado de sucesso
    /// </summary>
    public static Result<T> Success(T value) => new(true, value, Error.None);

    /// <summary>
    /// Cria um resultado de falha
    /// </summary>
    public static Result<T> Failure(Error error) => new(false, default, error);

    /// <summary>
    /// Cria um resultado de falha com mensagem
    /// </summary>
    public static Result<T> Failure(string message, ErrorType type = ErrorType.Failure)
        => new(false, default, new Error("Error.Failure", message, type));

    /// <summary>
    /// Converte Result<T> em Result
    /// </summary>
    public Result ToResult() => IsSuccess ? Result.Success() : Result.Failure(Error);
}

/// <summary>
/// Result Pattern para operações sem retorno de valor
/// </summary>
public class Result
{
    public bool IsSuccess { get; }
    public Error Error { get; }

    private Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException("Erro inválido para um resultado de sucesso");

        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException("Erro obrigatório para um resultado de falha");

        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Cria um resultado de sucesso
    /// </summary>
    public static Result Success() => new(true, Error.None);

    /// <summary>
    /// Cria um resultado de falha
    /// </summary>
    public static Result Failure(Error error) => new(false, error);

    /// <summary>
    /// Cria um resultado de falha com mensagem
    /// </summary>
    public static Result Failure(string message, ErrorType type = ErrorType.Failure)
        => new(false, new Error("Error.Failure", message, type));
}
