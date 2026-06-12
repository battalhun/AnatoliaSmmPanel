using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

public class UserStatsViewComponent : ViewComponent
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserStatsViewComponent(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public IViewComponentResult Invoke()
    {
        var count = _userManager.Users.Count();
        return View(count);
    }
}