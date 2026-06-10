using LibraryManagement.Entities.Common;

namespace LibraryManagement.Entities.Concrete;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Navigation Property: Bir kategoriye birçok kitap atanabilir (many-to-many)
    public ICollection<Book> Books { get; set; } = new List<Book>();
}