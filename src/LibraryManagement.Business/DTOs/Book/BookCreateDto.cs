namespace LibraryManagement.Business.DTOs.Book;

public class BookCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public int PageCount { get; set; }
    public int StockQuantity { get; set; }
    public string? Description { get; set; }
    public int PublisherId { get; set; }
    public List<int> AuthorIds { get; set; } = new();
    public List<int> CategoryIds { get; set; } = new();
}