namespace AnatoliaSmmPanel.Areas.Admin.ViewModels.Users;

public class UpdateUserViewModel
{
    public int Id { get; set; }
    public string? Nickname { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string Status { get; set; }
    public decimal Balance { get; set; }
    public decimal Spent { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
}