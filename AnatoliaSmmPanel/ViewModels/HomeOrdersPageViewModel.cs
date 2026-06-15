namespace AnatoliaSmmPanel.ViewModels;

public class HomeOrdersPageViewModel
{
    public List<HomeOrderListItemViewModel> Orders { get; set; } = new();

    public string Status { get; set; } = "all";
    public string? Search { get; set; }

    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public int TotalCount { get; set; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
