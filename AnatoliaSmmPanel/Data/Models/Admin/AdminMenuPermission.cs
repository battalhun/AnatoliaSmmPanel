namespace AnatoliaSmmPanel.Data.Models.Admin;

public class AdminMenuPermission
{
    public int Id { get; set; }

    public int AdminMenuId { get; set; }

    public AdminMenu AdminMenu { get; set; } = null!;

    public string RoleName { get; set; } = string.Empty;

}

