namespace AnatoliaSmmPanel.Areas.Admin.ViewModels.Services;
public class CreateCategoryViewModel
{
    public string Name { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
