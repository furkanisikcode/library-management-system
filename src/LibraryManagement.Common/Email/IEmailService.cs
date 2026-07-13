namespace LibraryManagement.Common.Email;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string body);
}