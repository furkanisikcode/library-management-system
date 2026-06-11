using System.Net;
using System.Text.Json;
using LibraryManagement.Business.Exceptions;

namespace LibraryManagement.WebAPI.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        int statusCode;
        string title;
        string detail;

        switch (exception)
        {
            case AppException appEx:
                statusCode = appEx.StatusCode;
                title = GetTitleForStatusCode(statusCode);
                detail = appEx.Message;
                _logger.LogWarning(exception, "İş kuralı hatası: {Message}", exception.Message);
                break;

            default:
                statusCode = (int)HttpStatusCode.InternalServerError;
                title = "Sunucu Hatası";
                detail = "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.";
                _logger.LogError(exception, "Beklenmeyen hata: {Message}", exception.Message);
                break;
        }

        context.Response.StatusCode = statusCode;

        var response = new
        {
            type = $"https://httpstatuses.com/{statusCode}",
            title,
            status = statusCode,
            detail,
            traceId = context.TraceIdentifier
        };

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }

    private static string GetTitleForStatusCode(int statusCode) => statusCode switch
    {
        400 => "Geçersiz İstek",
        404 => "Bulunamadı",
        409 => "Çakışma",
        _ => "Hata"
    };
}