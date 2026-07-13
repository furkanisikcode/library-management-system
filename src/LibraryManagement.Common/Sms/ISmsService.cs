namespace LibraryManagement.Common.Sms;

public interface ISmsService
{
    Task SendAsync(string phoneNumber, string message);
}