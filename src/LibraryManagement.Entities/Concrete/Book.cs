using LibraryManagement.Entities.Common;

namespace LibraryManagement.Entities.Concrete;

public class Book : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public int PageCount { get; set; }
    public int StockQuantity { get; set; }
    public string? Description { get; set; }

    // Foreign Key: Yayınevi (one-to-many'nin "many" tarafı)
    public int PublisherId { get; set; }
    public Publisher Publisher { get; set; } = null!;

    // Navigation Properties
    public ICollection<Author> Authors { get; set; } = new List<Author>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}