using app_ensinai.Modules.Media.Infrastructure.Setups;
using app_ensinai.Modules.Media.Domain.Interfaces.Repositories;
using app_ensinai.Modules.Media.Infrastructure.Repositories;
using app_ensinai.Modules.Media.Domain.UseCases;
using app_ensinai.Modules.Media.Application.Handlers;
using app_ensinai.Modules.Media.Application.Validators;

namespace app_ensinai.Modules.Media.Infrastructure;

public static class Setup
{
    public static void AddFileModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddS3Extensions(configuration);
        services.AddHandlers();
        services.AddUseCases();
        services.AddScoped<IFileRepository, FileRepository>();
    }

    private static void AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<UploadFileCommandHandler>();
        services.AddScoped<DeleteFileCommandHandler>();
        services.AddScoped<GetFileByIdQueryHandler>();

        // Registrar Validators
        services.AddScoped<FileUploadValidator>();
    }

    private static void AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IUploadFileUseCase, UploadFileUseCase>();
        services.AddScoped<IDeleteFileUseCase, DeleteFileUseCase>();
        services.AddScoped<IGetFileUseCase, GetFileUseCase>();
    }
}
