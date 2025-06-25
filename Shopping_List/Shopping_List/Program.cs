
using Microsoft.EntityFrameworkCore;
using DAL.ContextDir;
using BLL;
using DAL;
using IBL;
using IDAL;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Services.AddControllers();
builder.Services.AddDbContext<ShoppingListContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    options.UseSqlServer(connectionString, sqlServerOptions =>
    {
        sqlServerOptions.MigrationsAssembly("Shopping_List");
        sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });

    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});
builder.Services.AddSingleton<AutoMapper.IMapper>(provider =>
{
    var configuration = new AutoMapper.MapperConfiguration(cfg =>
    {
        cfg.AddProfile<BLL.MappingProfile>();
    });
    return configuration.CreateMapper();
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedOrigins", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "https://localhost:3000",
                "http://localhost:3001",
                "https://localhost:3001",
                "https://blue-ground-0e42e7610.6.azurestaticapps.net"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddScoped<IShoppingBLL, ShoppingBLL>();
builder.Services.AddScoped<ICategoryBLL, CategoryBLL>();
builder.Services.AddScoped<IShoppingDAL, ShoppingDAL>();
builder.Services.AddScoped<ICategoryDAL, CategoryDAL>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowedOrigins"); 
}
else
{
    app.UseCors("AllowedOrigins"); 
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        var context = scope.ServiceProvider.GetRequiredService<ShoppingListContext>();

        logger.LogInformation("Testing database connection...");
        var canConnect = await context.Database.CanConnectAsync();
        logger.LogInformation("Database connection test: {Status}", canConnect ? "SUCCESS" : "FAILED");

        if (!canConnect)
        {
            logger.LogWarning("Database connection failed, but application will continue to start");
        }

        if (app.Environment.IsDevelopment())
        {
            logger.LogInformation("Development mode: Database ready for migrations");
        }
    }
    catch (Exception ex)
    {
        var errorLogger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        errorLogger.LogError(ex, "Database connection error: {Message}", ex.Message);
        errorLogger.LogWarning("Application will continue to start despite database connection issues");
    }
}
app.MapGet("/", () => new
{
    Status = "Running",
    Timestamp = DateTime.UtcNow,
    Environment = app.Environment.EnvironmentName,
    Message = "Shopping List API is running successfully"
});

app.MapGet("/health", async (HttpContext context) =>
{
    try
    {
        using var scope = context.RequestServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ShoppingListContext>();
        var canConnect = await dbContext.Database.CanConnectAsync();

        var result = new
        {
            Status = canConnect ? "Healthy" : "Unhealthy",
            Database = canConnect ? "Connected" : "Disconnected",
            Timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsJsonAsync(result);
    }
    catch (Exception ex)
    {
        var result = new
        {
            Status = "Unhealthy",
            Database = "Error",
            Error = ex.Message,
            Timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsJsonAsync(result);
    }
});

var appLogger = app.Services.GetRequiredService<ILogger<Program>>();
appLogger.LogInformation("Shopping List API is starting...");
appLogger.LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);

app.Run();