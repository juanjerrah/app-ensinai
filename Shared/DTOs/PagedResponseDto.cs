using app_ensinai.Shared.Patterns;

namespace app_ensinai.Shared.DTOs;

/// <summary>
/// DTO genérico para respostas paginadas
/// </summary>
public class PagedResponseDto<T>
{
    public bool IsSuccess { get; set; }
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public PagedInfo Pagination { get; set; } = null!;
}
