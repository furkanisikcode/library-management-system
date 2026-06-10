namespace LibraryManagement.Business.DTOs.Book;

public class BookListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public int StockQuantity { get; set; }
    public string PublisherName { get; set; } = string.Empty;
}