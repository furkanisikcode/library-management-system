using Microsoft.Extensions.Logging;

namespace LibraryManagement.Common.Sms;

// Geliştirme/test için: gerçekten SMS atmaz, log'a yazar
public class ConsoleSmsService : ISmsService
{
    private readonly ILogger<ConsoleSmsService> _logger;

    public ConsoleSmsService(ILogger<ConsoleSmsService> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(string phoneNumber, string message)
    {
        _logger.LogInformation(
            "📱 SMS gönderildi (simülasyon) → Numara: {PhoneNumber} | Mesaj: {Message}",
            phoneNumber, message);

        return Task.CompletedTask;
    }
}