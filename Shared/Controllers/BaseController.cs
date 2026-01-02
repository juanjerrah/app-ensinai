using Microsoft.AspNetCore.Mvc;
using app_ensinai.Shared.Patterns;

namespace app_ensinai.Shared.Controllers;

/// <summary>
/// Controller base com métodos para padronizar respostas usando Result Pattern
/// </summary>
[ApiController]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Converte Result em IActionResult com tratamento automático de erros
    /// </summary>
    protected IActionResult HandleResult(Result result)
    {
        if (!result.IsSuccess)
        {
            return result.Error.Type switch
            {
                ErrorType.Validation => BadRequest(new { isSuccess = false, error = result.Error.Message }),
                ErrorType.NotFound => NotFound(new { isSuccess = false, error = result.Error.Message }),
                ErrorType.Unauthorized => StatusCode(403, new { isSuccess = false, error = result.Error.Message }),
                ErrorType.Conflict => Conflict(new { isSuccess = false, error = result.Error.Message }),
                _ => StatusCode(500, new { isSuccess = false, error = result.Error.Message })
            };
        }

        return Ok(new { isSuccess = true });
    }

    /// <summary>
    /// Converte Result em IActionResult com dados adicionais no sucesso
    /// </summary>
    protected IActionResult HandleResult(Result result, object? successData)
    {
        if (!result.IsSuccess)
        {
            return result.Error.Type switch
            {
                ErrorType.Validation => BadRequest(new { isSuccess = false, error = result.Error.Message }),
                ErrorType.NotFound => NotFound(new { isSuccess = false, error = result.Error.Message }),
                ErrorType.Unauthorized => StatusCode(403, new { isSuccess = false, error = result.Error.Message }),
                ErrorType.Conflict => Conflict(new { isSuccess = false, error = result.Error.Message }),
                _ => StatusCode(500, new { isSuccess = false, error = result.Error.Message })
            };
        }

        return Ok(new { isSuccess = true, data = successData });
    }

    /// <summary>
    /// Converte Result<T> em IActionResult com tratamento automático de erros
    /// </summary>
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (!result.IsSuccess)
        {
            return result.Error.Type switch
            {
                ErrorType.Validation => BadRequest(new { isSuccess = false, error = result.Error.Message }),
                ErrorType.NotFound => NotFound(new { isSuccess = false, error = result.Error.Message }),
                ErrorType.Unauthorized => StatusCode(403, new { isSuccess = false, error = result.Error.Message }),
                ErrorType.Conflict => Conflict(new { isSuccess = false, error = result.Error.Message }),
                _ => StatusCode(500, new { isSuccess = false, error = result.Error.Message })
            };
        }

        return Ok(new { isSuccess = true, data = result.Value });
    }

    /// <summary>
    /// Converte PagedResult<T> em IActionResult com tratamento automático de erros
    /// </summary>
    protected IActionResult HandlePagedResult<T>(PagedResult<T> result)
    {
        if (!result.IsSuccess)
        {
            return result.Error.Type switch
            {
                ErrorType.Validation => BadRequest(new { isSuccess = false, error = result.Error.Message }),
                ErrorType.NotFound => NotFound(new { isSuccess = false, error = result.Error.Message }),
                ErrorType.Unauthorized => StatusCode(403, new { isSuccess = false, error = result.Error.Message }),
                ErrorType.Conflict => Conflict(new { isSuccess = false, error = result.Error.Message }),
                _ => StatusCode(500, new { isSuccess = false, error = result.Error.Message })
            };
        }

        return Ok(new 
        { 
            isSuccess = true, 
            items = result.Items, 
            pagination = result.PageInfo 
        });
    }
}
