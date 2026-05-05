using AnatoliaSmmPanel.Enums;

public class NavigationTargetViewModel
{
    public string? Controller { get; set; }

    public string? Action { get; set; }

    public string? Page { get; set; }

    public string? Area { get; set; }

    public string? Url { get; set; }

    public string? PartialView { get; set; }

    public MenuConnect MenuConnect { get; set; }
}