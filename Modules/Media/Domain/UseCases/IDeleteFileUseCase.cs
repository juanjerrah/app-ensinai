using app_ensinai.Shared.Patterns;

namespace app_ensinai.Modules.Media.Domain.UseCases;

public interface IDeleteFileUseCase
{
    Task<Result> ExecuteAsync(Guid fileId);
}
