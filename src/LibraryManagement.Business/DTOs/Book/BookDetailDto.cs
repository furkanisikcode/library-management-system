namespace LibraryManagement.Business.DTOs.Book;

public class BookDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public int PageCount { get; set; }
    public int StockQuantity { get; set; }
    public string? Description { get; set; }
    public string PublisherName { get; set; } = string.Empty;
    public List<string> AuthorNames { get; set; } = new();
    public List<string> CategoryNames { get; set; } = new();
    public DateTime CreatedDate { get; set; }
}