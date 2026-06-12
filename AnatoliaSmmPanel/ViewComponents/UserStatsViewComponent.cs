using AnatoliaSmmPanel.Data.Models.Appliciton;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class UserStatsViewComponent : ViewComponent
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserStatsViewComponent(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public IViewComponentResult Invoke()
    {
        var count = _userManager.Users.Count();
        return View(count);
    }
}