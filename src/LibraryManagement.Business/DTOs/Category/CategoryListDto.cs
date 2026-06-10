namespace LibraryManagement.Business.DTOs.Category;

public class CategoryListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int BookCount { get; set; }
}