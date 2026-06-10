namespace LibraryManagement.Business.DTOs.Publisher;

public class PublisherListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Website { get; set; }
    public int BookCount { get; set; }
}