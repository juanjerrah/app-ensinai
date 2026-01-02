using System.Diagnostics;
using System.Text;

namespace app_ensinai.Shared.Middlewares;

/// <summary>
/// Middleware para logging de requisições HTTP
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Gerar ID único para rastreamento da requisição
        var requestId = Guid.NewGuid().ToString();
        context.Items["RequestId"] = requestId;

        // Iniciar cronômetro
        var stopwatch = Stopwatch.StartNew();

        // Capturar informações da requisição
        var request = context.Request;
        var method = request.Method;
        var path = request.Path;
        var queryString = request.QueryString.ToString();
        var userAgent = request.Headers["User-Agent"].ToString();
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();

        // Log de início da requisição
        _logger.LogInformation(
            "[{RequestId}] Iniciando requisição: {Method} {Path}{QueryString} | IP: {IpAddress} | UserAgent: {UserAgent}",
            requestId, method, path, queryString, ipAddress, userAgent);

        // Capturar body da requisição (se houver e não for arquivo)
        var requestBody = await GetRequestBodyAsync(request);
        if (!string.IsNullOrEmpty(requestBody))
            _logger.LogDebug("[{RequestId}] Request Body: {RequestBody}", requestId, requestBody);
        

        // Capturar o response body original
        var originalBodyStream = context.Response.Body;

        try
        {
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            // Processar a requisição
            await _next(context);

            stopwatch.Stop();

            // Capturar informações da resposta
            var statusCode = context.Response.StatusCode;
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            // Capturar body da resposta
            var responseBodyText = await GetResponseBodyAsync(responseBody);

            // Log de fim da requisição
            var logLevel = statusCode >= 500 ? LogLevel.Error :
                          statusCode >= 400 ? LogLevel.Warning :
                          LogLevel.Information;

            _logger.Log(logLevel,
                "[{RequestId}] Requisição finalizada: {Method} {Path} | Status: {StatusCode} | Tempo: {ElapsedMs}ms",
                requestId, method, path, statusCode, elapsedMilliseconds);

            if (!string.IsNullOrEmpty(responseBodyText) && statusCode >= 400)
            {
                _logger.LogDebug("[{RequestId}] Response Body: {ResponseBody}", requestId, responseBodyText);
            }

            // Copiar o conteúdo capturado para o stream original
            await responseBody.CopyToAsync(originalBodyStream);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            
            _logger.LogError(ex,
                "[{RequestId}] Erro na requisição: {Method} {Path} | Tempo: {ElapsedMs}ms | Erro: {ErrorMessage}",
                requestId, method, path, stopwatch.ElapsedMilliseconds, ex.Message);

            throw;
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }

    private async Task<string> GetRequestBodyAsync(HttpRequest request)
    {
        // Verificar se é multipart/form-data (upload de arquivo)
        if (request.ContentType?.Contains("multipart/form-data") == true)
        {
            return "[Multipart Form Data - Arquivo]";
        }

        // Verificar se tem body e é JSON/texto
        if (request.ContentLength > 0 && 
            (request.ContentType?.Contains("application/json") == true || 
             request.ContentType?.Contains("text") == true))
        {
            request.EnableBuffering();
            
            using var reader = new StreamReader(
                request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: true);

            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;

            // Limitar tamanho do log (máximo 1000 caracteres)
            return body.Length > 1000 ? body.Substring(0, 1000) + "..." : body;
        }

        return string.Empty;
    }

    private async Task<string> GetResponseBodyAsync(MemoryStream responseBody)
    {
        responseBody.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(responseBody).ReadToEndAsync();
        responseBody.Seek(0, SeekOrigin.Begin);

        // Limitar tamanho do log (máximo 1000 caracteres)
        return text.Length > 1000 ? text.Substring(0, 1000) + "..." : text;
    }
}

/// <summary>
/// Extension method para registrar o middleware
/// </summary>
public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}
