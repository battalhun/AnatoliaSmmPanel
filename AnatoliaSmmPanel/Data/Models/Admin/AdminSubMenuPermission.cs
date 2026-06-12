namespace AnatoliaSmmPanel.Data.Models.Admin;

public class AdminSubMenuPermission
{
    public int Id { get; set; }

    public int AdminSubMenuId { get; set; }

    public AdminSubMenu AdminSubMenu { get; set; } = null!;

    public string RoleName { get; set; } = string.Empty;
}

