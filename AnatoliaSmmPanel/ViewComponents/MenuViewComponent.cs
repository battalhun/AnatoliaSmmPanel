using AnatoliaSmmPanel.Data;
using AnatoliaSmmPanel.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class MenuViewComponent : ViewComponent
{
    private readonly HomeContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public MenuViewComponent(
        HomeContext context,
        UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);

        var userRoles = user != null
            ? await _userManager.GetRolesAsync(user)
            : new List<string>();


        var menus = await _context.Menus
            .Where(x => x.IsActive)
            .Include(x => x.NavigationTarget)
            .Include(x => x.MenuPermissions)
            .OrderBy(x => x.Order)
            .Select(x => new MenuViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Icon = x.Icon,
                Order = x.Order,
                OpenInNewTab = x.OpenInNewTab,

                NavigationTarget = new NavigationTargetViewModel
                {
                    Controller = x.NavigationTarget.Controller,
                    Action = x.NavigationTarget.Action,
                    Page = x.NavigationTarget.Page,
                    Area = x.NavigationTarget.Area,
                    Url = x.NavigationTarget.Url,
                    PartialView = x.NavigationTarget.PartialView,
                    MenuConnect = x.NavigationTarget.MenuConnect
                },

                IsVisible =
                    userRoles.Contains("SuperAdmin") ||
                    x.MenuPermissions == null ||
                    !x.MenuPermissions.Any() ||
                    x.MenuPermissions.Any(p => userRoles.Contains(p.RoleName))
            })
            .Where(x => x.IsVisible)
            .ToListAsync();

        return View(menus);
    }
}