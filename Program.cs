using appAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using DotNetEnv;  // Asegúrate de añadir esta línea

var builder = WebApplication.CreateBuilder(args);

// Cargar variables de entorno desde el archivo .env
Env.Load();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder
            .AllowAnyOrigin()  // Permitir cualquier origen
            .AllowAnyMethod()  // Permitir cualquier método HTTP (GET, POST, etc.)
            .AllowAnyHeader();  // Permitir cualquier encabezado
    });
});

// Agregar servicios a la aplicación
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de la base de datos
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
            npgsqlOptions.CommandTimeout(30);
        }));

// Agregar Health Checks ANTES de builder.Build()
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("Database");

var app = builder.Build();

// Configuración del middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseAuthorization();

// Mapear los endpoints incluyendo health
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

// Agregar esta nueva clase en tu proyecto (puede ser en un archivo nuevo llamado DatabaseHealthCheck.cs)
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IDbContextFactory<MyDbContext> _dbContextFactory;
    private readonly MyDbContext _dbContext;

    public DatabaseHealthCheck(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.Database.CanConnectAsync(cancellationToken);
            return HealthCheckResult.Healthy("Database connection is healthy");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database connection is unhealthy", ex);
        }
    }
}
