using LibraryManagement.Entities.Common;

namespace LibraryManagement.Entities.Concrete;

public class Author : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Biography { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Nationality { get; set; }

    // Navigation Property: Bir yazarın birçok kitabı olabilir (many-to-many)
    public ICollection<Book> Books { get; set; } = new List<Book>();
}