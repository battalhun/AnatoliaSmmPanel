using AnatoliaSmmPanel.Models;

namespace AnatoliaSmmPanel.ViewModels;

public class MenuPermissionViewModel
{
    public int Id { get; set; }

    public int MenuId { get; set; }

    public string RoleName { get; set; } = string.Empty;
}

