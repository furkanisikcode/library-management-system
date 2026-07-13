using LibraryManagement.WebAPI.Middlewares;
using LibraryManagement.Business.Settings;
using LibraryManagement.Business.Services.Abstract;
using LibraryManagement.Business.Services.Concrete;
using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryManagement.Business.Mappings;
using LibraryManagement.Business.Validators;
using LibraryManagement.DataAccess.Context;
using LibraryManagement.DataAccess.Repositories.Abstract;
using LibraryManagement.DataAccess.Repositories.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using LibraryManagement.Common.Email;
using LibraryManagement.Common.Sms;

// Bootstrap logger (uygulama başlayana kadar minimal log)
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
// Serilog'u appsettings.json'daki config ile yapılandır
// Serilog'u appsettings.json'daki config ile yapılandır (testte atla)
if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Host.UseSerilog((context, services, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));
}

builder.Services.AddControllers();

// Configuration
builder.Services.Configure<LoanSettings>(
    builder.Configuration.GetSection("LoanSettings"));

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

// DbContext
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Generic Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// AutoMapper + Validators
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(BookProfile).Assembly));
builder.Services.AddValidatorsFromAssemblyContaining<BookCreateDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();

// Repositories
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IPenaltyRepository, PenaltyRepository>();

// Services
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddScoped<IPenaltyService, PenaltyService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
// Common - Email
builder.Services.AddScoped<IEmailService, ConsoleEmailService>();
builder.Services.AddScoped<ISmsService, ConsoleSmsService>();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwtSettings!.Secret);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Swagger (basit)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// HTTP request loglaması
if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseSerilogRequestLogging();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsEnvironment("Testing"))
{
    // Test host'unda try/finally + CloseAndFlush çalıştırma (dispose sorununu önler)
    app.Run();
}
else
{
    try
    {
        Log.Information("Library Management API başlatılıyor...");
        app.Run();
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "Uygulama başlatılırken hata oluştu.");
    }
    finally
    {
        Log.CloseAndFlush();
    }
}

public partial class Program { }