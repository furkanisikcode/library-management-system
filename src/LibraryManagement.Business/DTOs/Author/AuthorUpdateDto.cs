namespace LibraryManagement.Business.DTOs.Author;

public class AuthorUpdateDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Biography { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Nationality { get; set; }
}