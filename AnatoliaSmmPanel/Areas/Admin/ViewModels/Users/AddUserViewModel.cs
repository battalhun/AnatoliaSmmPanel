namespace AnatoliaSmmPanel.Areas.Admin.ViewModels.Users;

public class AddUserViewModel
{
    public string Nickname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string Status { get; set; } = "Active";
    public decimal Balance { get; set; } = 0;
}