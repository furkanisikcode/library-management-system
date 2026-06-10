namespace LibraryManagement.Business.DTOs.Author;

public class AuthorListDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Nationality { get; set; }
    public int BookCount { get; set; }
}