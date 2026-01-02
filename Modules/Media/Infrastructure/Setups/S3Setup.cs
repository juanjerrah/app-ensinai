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
        var serviceUrl = configuration["AWS.S3.SERVICEURL"]; // Para LocalStack
        var forcePathStyle = configuration.GetValue("AWS.S3.FORCEPATHHSTYLE", false);


        // Se ServiceURL estiver configurado, usar LocalStack
        if (!string.IsNullOrEmpty(serviceUrl))
        {
            Console.WriteLine($"✅ AWS S3 configurado com LocalStack: {serviceUrl}");

            services.AddSingleton<IAmazonS3>(sp =>
            {
                var config = new Amazon.S3.AmazonS3Config
                {
                    ServiceURL = serviceUrl,
                    ForcePathStyle = forcePathStyle,
                    AuthenticationRegion = region
                };

                return new AmazonS3Client(accessKey, secretKey, config);
            });
        }
        else
        {
            Console.WriteLine("✅ AWS S3 configurado com credenciais AWS reais");
            var awsOptions = new AWSOptions
            {
                Region = Amazon.RegionEndpoint.GetBySystemName(region),
                Credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey)
            };
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonS3>();
        }

        services.AddScoped<IS3Service, S3Service>();

        return services;
    }
}
