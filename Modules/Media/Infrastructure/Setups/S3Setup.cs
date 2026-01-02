using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using app_ensinai.Modules.Media.Domain.Interfaces.Services;
using app_ensinai.Modules.Media.Infrastructure.Services;

namespace app_ensinai.Modules.Media.Infrastructure.Setups;

public static class S3Setup
{
    public static IServiceCollection AddS3Extensions(this IServiceCollection services, IConfiguration configuration)
    {
        var region = configuration["AWS.REGION"]
            ?? throw new ArgumentNullException("AWS.REGION", "Variável de ambiente AWS.REGION não configurada no launchSettings.json");

        var accessKey = configuration["AWS.S3.ACCESSKEY"];
        var secretKey = configuration["AWS.S3.SECRETKEY"];

        // Configura o cliente S3 da AWS
        var awsOptions = new AWSOptions
        {
            Region = Amazon.RegionEndpoint.GetBySystemName(region),
            Credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey)
        };

        Console.WriteLine("✅ AWS S3 configurado com credenciais específicas");

        services.AddDefaultAWSOptions(awsOptions);
        services.AddAWSService<IAmazonS3>();

        // Registra o serviço S3
        services.AddScoped<IS3Service, S3Service>();

        return services;
    }
}
