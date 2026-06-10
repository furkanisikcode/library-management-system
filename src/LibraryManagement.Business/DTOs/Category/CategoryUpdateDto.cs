namespace LibraryManagement.Business.DTOs.Category;

public class CategoryUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}