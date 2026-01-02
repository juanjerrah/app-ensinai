using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using app_ensinai.Modules.Media.Domain.Interfaces.Services;

namespace app_ensinai.Modules.Media.Infrastructure.Services;

public class S3Service : IS3Service
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3Service(IAmazonS3 s3Client, IConfiguration configuration)
    {
        _s3Client = s3Client;
        
        _bucketName = configuration["AWS.S3.BUCKETNAME"] 
            ?? throw new ArgumentNullException("AWS.S3.BUCKETNAME", "Variável de ambiente AWS.S3.BUCKETNAME não configurada no launchSettings.json");
    }

    /// <summary>
    /// Faz upload de um arquivo para o S3
    /// </summary>
    /// <param name="fileStream">Stream do arquivo</param>
    /// <param name="fileName">Nome do arquivo no S3</param>
    /// <param name="contentType">Tipo de conteúdo (ex: image/jpeg, application/pdf)</param>
    /// <returns>URL do arquivo no S3</returns>
    public async Task<(string fileUrl, string bucketName)> UploadFileAsync(Stream fileStream, string fileName, string? contentType = null, bool isPrivate = false)
    {
        try
        {
            var uploadRequest = new PutObjectRequest
            {
                InputStream = fileStream,
                Key = fileName,
                BucketName = _bucketName,
                ContentType = contentType ?? "application/octet-stream",
                CannedACL = isPrivate ? S3CannedACL.Private : S3CannedACL.PublicRead
            };

            await _s3Client.PutObjectAsync(uploadRequest);

            return (GetFileUrl(fileName, isPrivate), _bucketName);
        }
        catch (AmazonS3Exception ex)
        {
            throw new Exception($"Erro ao fazer upload para S3: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Deleta um arquivo do S3
    /// </summary>
    /// <param name="fileName">Nome do arquivo no S3</param>
    /// <returns>True se deletado com sucesso</returns>
    public async Task<bool> DeleteFileAsync(string fileName)
    {
        try
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };

            await _s3Client.DeleteObjectAsync(deleteRequest);
            return true;
        }
        catch (AmazonS3Exception ex)
        {
            throw new Exception($"Erro ao deletar arquivo do S3: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Baixa um arquivo do S3
    /// </summary>
    /// <param name="fileName">Nome do arquivo no S3</param>
    /// <returns>Stream do arquivo</returns>
    public async Task<Stream> DownloadFileAsync(string fileName)
    {
        try
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };

            var response = await _s3Client.GetObjectAsync(request);
            
            // Copia o stream de resposta para um MemoryStream para evitar problemas com disposição
            var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            
            return memoryStream;
        }
        catch (AmazonS3Exception ex)
        {
            throw new Exception($"Erro ao baixar arquivo do S3: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Verifica se um arquivo existe no S3
    /// </summary>
    /// <param name="fileName">Nome do arquivo no S3</param>
    /// <returns>True se o arquivo existe</returns>
    public async Task<bool> FileExistsAsync(string fileName)
    {
        try
        {
            var request = new GetObjectMetadataRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };

            await _s3Client.GetObjectMetadataAsync(request);
            return true;
        }
        catch (AmazonS3Exception ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;
                
            throw new Exception($"Erro ao verificar existência do arquivo no S3: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Gera uma URL pré-assinada para acesso temporário a um arquivo privado no S3
    /// </summary>
    /// <param name="fileName">Nome do arquivo no S3</param>
    /// <param name="expirationMinutes">Tempo de expiração da URL em minutos (padrão: 60)</param>
    /// <returns>URL pré-assinada do arquivo</returns>
    public string GeneratePresignedUrl(string fileName, int expirationMinutes = 60)
    {
        try
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = fileName,
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                Verb = HttpVerb.GET
            };

            return _s3Client.GetPreSignedURL(request);
        }
        catch (AmazonS3Exception ex)
        {
            throw new Exception($"Erro ao gerar URL pré-assinada: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Gera uma URL de acordo com o tipo de acesso do arquivo (público ou privado)
    /// </summary>
    /// <param name="fileName">Nome do arquivo no S3</param>
    /// <param name="isPrivate">Se o arquivo é privado (gera URL pré-assinada) ou público</param>
    /// <param name="expirationMinutes">Tempo de expiração em minutos (apenas para arquivos privados)</param>
    /// <returns>URL do arquivo</returns>
    public string GetFileUrl(string fileName, bool isPrivate = false, int expirationMinutes = 60)
    {
        return isPrivate 
            ? GeneratePresignedUrl(fileName, expirationMinutes)
            : GetPublicUrl(fileName);
    }
    private string GetPublicUrl(string fileName)
    {
        return $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
    }
}