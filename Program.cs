using app_ensinai.Modules.Media.Infrastructure;
using app_ensinai.Shared.CrossCutting;
using app_ensinai.Shared.Infrastructure.Dapper;
using app_ensinai.Shared.Middlewares;
using Microsoft.OpenApi;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configurar Dapper
DapperConfiguration.Configure();

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configurar enums para serem serializados como strings
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Swagger/OpenAPI Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EnsinAI API",
        Version = "v1",
        Description = "API para aplicação de agendamento de aulas particulares",
        Contact = new OpenApiContact
        {
            Name = "EnsinAI Team",
            Email = "contato@ensinai.com"
        }
    });

    // Habilitar anotações XML (comentários)
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Add PostgreSQL Configuration
builder.Services.AddPostgresConfiguration(builder.Configuration);

// Modules
builder.Services.AddFileModule(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "EnsinAI API v1");
        options.RoutePrefix = "swagger"; // Acessível em: /swagger
        options.DocumentTitle = "EnsinAI API Documentation";
        options.DisplayRequestDuration();
    });
}

// Middleware de logging (deve ser um dos primeiros)
app.UseRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Health check endpoint para Docker e monitoramento
app.MapGet("/health", () => Results.Ok(new 
{ 
    status = "healthy", 
    timestamp = DateTime.UtcNow,
    environment = app.Environment.EnvironmentName
}))
.WithName("HealthCheck")
.WithTags("Health")
.Produces(200);

app.Run();
