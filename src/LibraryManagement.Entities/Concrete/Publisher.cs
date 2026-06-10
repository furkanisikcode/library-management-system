using LibraryManagement.Entities.Common;

namespace LibraryManagement.Entities.Concrete;

public class Publisher : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Website { get; set; }

    // Navigation Property: Bir yayınevinin birçok kitabı vardır (one-to-many)
    public ICollection<Book> Books { get; set; } = new List<Book>();
}