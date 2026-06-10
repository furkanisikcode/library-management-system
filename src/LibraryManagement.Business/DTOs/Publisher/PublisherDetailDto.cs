namespace LibraryManagement.Business.DTOs.Publisher;

public class PublisherDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Website { get; set; }
    public int BookCount { get; set; }
    public DateTime CreatedDate { get; set; }
}