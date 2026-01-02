namespace app_ensinai.Shared.Patterns;

/// <summary>
/// Result Pattern para operações paginadas
/// </summary>
public class PagedResult<T>
{
    public bool IsSuccess { get; }
    public IEnumerable<T> Items { get; }
    public PagedInfo PageInfo { get; }
    public Error Error { get; }

    private PagedResult(bool isSuccess, IEnumerable<T> items, PagedInfo pageInfo, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException("Erro inválido para um resultado de sucesso");

        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException("Erro obrigatório para um resultado de falha");

        IsSuccess = isSuccess;
        Items = items;
        PageInfo = pageInfo;
        Error = error;
    }

    /// <summary>
    /// Cria um resultado paginado de sucesso
    /// </summary>
    public static PagedResult<T> Success(IEnumerable<T> items, int currentPage, int pageSize, int totalItems)
    {
        var pageInfo = new PagedInfo(currentPage, pageSize, totalItems);
        return new PagedResult<T>(true, items, pageInfo, Error.None);
    }

    /// <summary>
    /// Cria um resultado paginado de falha
    /// </summary>
    public static PagedResult<T> Failure(Error error)
        => new PagedResult<T>(false, Enumerable.Empty<T>(), new PagedInfo(0, 0, 0), error);

    /// <summary>
    /// Cria um resultado paginado de falha com mensagem
    /// </summary>
    public static PagedResult<T> Failure(string message, ErrorType type = ErrorType.Failure)
        => new PagedResult<T>(false, Enumerable.Empty<T>(), new PagedInfo(0, 0, 0), new Error("Error.Failure", message, type));
}
