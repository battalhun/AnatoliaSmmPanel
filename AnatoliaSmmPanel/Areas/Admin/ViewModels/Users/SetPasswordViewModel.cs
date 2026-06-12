namespace AnatoliaSmmPanel.Areas.Admin.ViewModels.Users;

public class SetPasswordViewModel
{
    public int UserId { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}