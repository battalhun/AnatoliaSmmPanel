namespace AnatoliaSmmPanel.Areas.Admin.ViewModels.Services;

public class EditServiceViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public decimal Rate { get; set; }
    public int Min { get; set; }
    public int Max { get; set; }
    public int SortOrder { get; set; }
    public bool Dripfeed { get; set; }
    public bool Refill { get; set; }
    public bool Cancel { get; set; }
    public bool IsActive { get; set; }
    public int ProviderId { get; set; }
    public int? ServiceCategoryId { get; set; }
}