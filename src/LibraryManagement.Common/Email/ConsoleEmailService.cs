using Microsoft.Extensions.Logging;

namespace LibraryManagement.Common.Email;

// Geliştirme/test için: gerçekten mail atmaz, log'a yazar
public class ConsoleEmailService : IEmailService
{
    private readonly ILogger<ConsoleEmailService> _logger;

    public ConsoleEmailService(ILogger<ConsoleEmailService> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(string to, string subject, string body)
    {
        _logger.LogInformation(
            "📧 E-posta gönderildi (simülasyon) → Kime: {To} | Konu: {Subject} | İçerik: {Body}",
            to, subject, body);

        return Task.CompletedTask;
    }
}