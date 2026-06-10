namespace LibraryManagement.Business.DTOs.Author;

public class AuthorDetailDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Biography { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Nationality { get; set; }
    public int BookCount { get; set; }
    public DateTime CreatedDate { get; set; }
}