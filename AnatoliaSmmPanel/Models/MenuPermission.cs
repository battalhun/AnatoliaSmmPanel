namespace AnatoliaSmmPanel.Models;

public class MenuPermission
{
    public int Id { get; set; }

    public int MenuId { get; set; }

    public Menu Menu { get; set; } = null!;

    public string RoleName { get; set; } = string.Empty;
}

