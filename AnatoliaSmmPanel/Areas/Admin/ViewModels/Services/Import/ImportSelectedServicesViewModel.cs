namespace AnatoliaSmmPanel.Areas.Admin.ViewModels.Services.Import;

public class ImportSelectedServicesViewModel
{
    public int SelectedProviderId { get; set; }

    public List<int> SelectedServiceIds { get; set; } = new();
}