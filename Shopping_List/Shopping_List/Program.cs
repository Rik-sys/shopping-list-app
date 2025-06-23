////using DAL.ContextDir;
////using Microsoft.EntityFrameworkCore;

////var builder = WebApplication.CreateBuilder(args);

////// Add services to the container.

////builder.Services.AddControllers();
////// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
////builder.Services.AddEndpointsApiExplorer();
////builder.Services.AddSwaggerGen();
////builder.Services.AddDbContext<ShoppingListContext>(options =>
////{
////    options.UseSqlServer(
////        builder.Configuration.GetConnectionString("hoppingListConnection"),
////        sqlOptions =>
////        {

////            sqlOptions.MigrationsAssembly("Shopping_List");  

////            sqlOptions.EnableRetryOnFailure(
////                maxRetryCount: 3,
////                maxRetryDelay: TimeSpan.FromSeconds(30),
////                errorNumbersToAdd: null);
////        });

////    if (builder.Environment.IsDevelopment())
////    {
////        options.EnableSensitiveDataLogging();
////        options.EnableDetailedErrors();
////    }
////});

////var app = builder.Build();

////// Configure the HTTP request pipeline.
////if (app.Environment.IsDevelopment())
////{
////    app.UseSwagger();
////    app.UseSwaggerUI();
////}

////app.UseHttpsRedirection();

////app.UseAuthorization();

////app.MapControllers();

////app.Run();
//// =====================================================
//// Complete Program.cs for API Project
//// File: Shopping_List/Program.cs
//// =====================================================

//using Microsoft.EntityFrameworkCore;
//using DAL.ContextDir;
//using BLL;
//using DAL;
//using IBL;
//using IDAL;



//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllers();

//// =====================================================
//// Database Configuration - FIXED
//// =====================================================
//builder.Services.AddDbContext<ShoppingListContext>(options =>
//{
//    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//    options.UseSqlServer(connectionString, sqlServerOptions =>
//    {
//        // זה החלק החשוב! אומר ל-EF איפה לשים את ה-migrations
//        sqlServerOptions.MigrationsAssembly("Shopping_List");

//        sqlServerOptions.EnableRetryOnFailure(
//            maxRetryCount: 3,
//            maxRetryDelay: TimeSpan.FromSeconds(30),
//            errorNumbersToAdd: null);
//    });

//    if (builder.Environment.IsDevelopment())
//    {
//        options.EnableSensitiveDataLogging();
//        options.EnableDetailedErrors();
//    }
//});

//// CORS Configuration
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowReactApp", policy =>
//    {
//        policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
//              .AllowAnyHeader()
//              .AllowAnyMethod()
//              .AllowCredentials();
//    });
//});

//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//// הוסף את השורה הזאת ל-Program.cs:
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddScoped<IShoppingBLL, ShoppingBLL>();
//builder.Services.AddScoped<ICategoryBLL, CategoryBLL>();

//// Data Access Layer (Scoped for per-request lifecycle)
//builder.Services.AddScoped<IShoppingDAL, ShoppingDAL>();
//builder.Services.AddScoped<ICategoryDAL, CategoryDAL>();


//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.UseCors("AllowReactApp");
//app.UseAuthorization();

//app.MapControllers();

//// =====================================================
//// Test Database Connection
//// =====================================================
//using (var scope = app.Services.CreateScope())
//{
//    try
//    {
//        var context = scope.ServiceProvider.GetRequiredService<ShoppingListContext>();

//        // Just test the connection
//        var canConnect = await context.Database.CanConnectAsync();
//        Console.WriteLine($"Database connection test: {(canConnect ? "SUCCESS" : "FAILED")}");

//        if (app.Environment.IsDevelopment())
//        {
//            Console.WriteLine("Development mode: Will create database if needed during migration");
//        }
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Database connection error: {ex.Message}");
//        // Don't throw - let the app start and handle migrations separately
//    }
//}

//app.Run();

//// =====================================================
//// Design Time DbContext Factory (אופציונלי לבעיות)
//// זה עוזר ל-EF למצוא את ה-DbContext בזמן design
//// =====================================================
///*
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using DAL.ContextDir;

//namespace Shopping_List
//{
//    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ShoppingListContext>
//    {
//        public ShoppingListContext CreateDbContext(string[] args)
//        {
//            var optionsBuilder = new DbContextOptionsBuilder<ShoppingListContext>();
            
//            // Connection string for design time
//            optionsBuilder.UseSqlServer(
//                "Server=(localdb)\\mssqllocaldb;Database=ShoppingListDB;Trusted_Connection=true;TrustServerCertificate=true;",
//                options => options.MigrationsAssembly("Shopping_List"));

//            return new ShoppingListContext(optionsBuilder.Options);
//        }
//    }
//}
//*/
// =====================================================
// Complete Fixed Program.cs
// Project: Shopping_List (API)
// =====================================================

using Microsoft.EntityFrameworkCore;
using DAL.ContextDir;
using BLL;
using DAL;
using IBL;
using IDAL;
using System.Reflection; // ? Add this for AppDomain

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// =====================================================
// Database Configuration
// =====================================================
builder.Services.AddDbContext<ShoppingListContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    
    options.UseSqlServer(connectionString, sqlServerOptions =>
    {
        // זה החלק החשוב! אומר ל-EF איפה לשים את ה-migrations
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

// =====================================================
// ? AutoMapper Registration
// =====================================================
// First, try this simple approach:
builder.Services.AddSingleton<AutoMapper.IMapper>(provider =>
{
    var configuration = new AutoMapper.MapperConfiguration(cfg =>
    {
        cfg.AddProfile<BLL.MappingProfile>();
    });
    return configuration.CreateMapper();
});

// =====================================================
// CORS Configuration
// =====================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",    // React Dev Server
                "https://localhost:3000",   // React HTTPS
                "http://localhost:3001",    // Alternative port
                "https://localhost:3001"    // Alternative HTTPS
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// =====================================================
// Dependency Injection
// =====================================================
// Business Logic Layer
builder.Services.AddScoped<IShoppingBLL, ShoppingBLL>();
builder.Services.AddScoped<ICategoryBLL, CategoryBLL>();

// Data Access Layer
builder.Services.AddScoped<IShoppingDAL, ShoppingDAL>();
builder.Services.AddScoped<ICategoryDAL, CategoryDAL>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthorization();
app.MapControllers();

// =====================================================
// Database Connection Test
// =====================================================
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<ShoppingListContext>();
        
        // Test the connection
        var canConnect = await context.Database.CanConnectAsync();
        Console.WriteLine($"Database connection test: {(canConnect ? "SUCCESS" : "FAILED")}");
        
        if (app.Environment.IsDevelopment())
        {
            Console.WriteLine("Development mode: Database ready for migrations");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database connection error: {ex.Message}");
        // Don't throw - let the app start
    }
}

Console.WriteLine("?? Shopping List API is starting...");
app.Run();

