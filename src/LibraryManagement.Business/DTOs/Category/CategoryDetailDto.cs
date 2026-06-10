namespace LibraryManagement.Business.DTOs.Category;

public class CategoryDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int BookCount { get; set; }
    public DateTime CreatedDate { get; set; }
}