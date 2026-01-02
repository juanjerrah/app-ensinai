namespace app_ensinai.Shared.Patterns;

/// <summary>
/// Informações de paginação
/// </summary>
public class PagedInfo
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;

    public PagedInfo(int currentPage, int pageSize, int totalItems)
    {
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalItems = totalItems;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
    }
}
