using app_ensinai.Modules.Media.Domain.UseCases;
using app_ensinai.Shared.Patterns;

namespace app_ensinai.Modules.Media.Application.Handlers;

public class DeleteFileCommandHandler
{
    private readonly IDeleteFileUseCase _deleteFileUseCase;

    public DeleteFileCommandHandler(IDeleteFileUseCase deleteFileUseCase)
    {
        _deleteFileUseCase = deleteFileUseCase;
    }

    public async Task<Result> HandleAsync(Guid fileId)
    {
        // Validação básica no Handler
        if (fileId == Guid.Empty)
            return Result.Failure("FileId é obrigatório.", ErrorType.Validation);
        
        return await _deleteFileUseCase.ExecuteAsync(fileId);
    }
}
