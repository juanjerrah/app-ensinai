using Microsoft.AspNetCore.Mvc;
using app_ensinai.Modules.Media.Application.Handlers;
using app_ensinai.Shared.Controllers;
using app_ensinai.Shared.Patterns;
using app_ensinai.Modules.Media.Application.DTOs;

namespace app_ensinai.Modules.Media.Presentation;

[Route("api/files")]
public class FileController : BaseController
{
    private readonly UploadFileCommandHandler _uploadFileHandler;
    private readonly DeleteFileCommandHandler _deleteFileHandler;
    private readonly GetFileByIdQueryHandler _getFileByIdHandler;
    private readonly ILogger<FileController> _logger;

    public FileController(
        UploadFileCommandHandler uploadFileHandler,
        DeleteFileCommandHandler deleteFileHandler,
        GetFileByIdQueryHandler getFileByIdHandler,
        ILogger<FileController> logger)
    {
        _uploadFileHandler = uploadFileHandler;
        _deleteFileHandler = deleteFileHandler;
        _getFileByIdHandler = getFileByIdHandler;
        _logger = logger;
    }

    /// <summary>
    /// Faz upload de um arquivo
    /// </summary>
    [HttpPost("upload")]
    [ProducesResponseType(typeof(Result<FileResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<FileResponseDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadFile([FromForm] FileUploadDto fileUploadDto)
    {
        var result = await _uploadFileHandler.HandleAsync(fileUploadDto);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Falha ao fazer upload de arquivo: {Error}", result.Error.Message);
            return HandleResult(result);
        }

        _logger.LogInformation("Arquivo {FileName} enviado com sucesso",
            result.Value!.FileName);

        return HandleResult(result);
    }

    /// <summary>
    /// Deleta um arquivo
    /// </summary>
    [HttpDelete("{fileId}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteFile(Guid fileId)
    {
        var result = await _deleteFileHandler.HandleAsync(fileId);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Falha ao deletar arquivo {FileId}: {Error}", fileId, result.Error.Message);
            return HandleResult(result, null);
        }

        _logger.LogInformation("Arquivo {FileId} deletado com sucesso", fileId);

        return HandleResult(result, new { message = "Arquivo deletado com sucesso", fileId });
    }

    /// <summary>
    /// Busca um arquivo por ID
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(Result<FileResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFileById([FromQuery] GetFileByIdDto getDto)
    {
        var result = await _getFileByIdHandler.HandleAsync(
            getDto.FileId, 
            getDto.IncludeUrl, 
            getDto.UrlExpirationMinutes);

        return HandleResult(result);
    }
}
