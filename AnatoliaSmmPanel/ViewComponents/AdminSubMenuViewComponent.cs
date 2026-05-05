using AnatoliaSmmPanel.Data;
using AnatoliaSmmPanel.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnatoliaSmmPanel.Areas.Admin.ViewModels;
public class AdminSubMenuViewComponent : ViewComponent
{
    private readonly HomeContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public AdminSubMenuViewComponent(
        HomeContext context,
        UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync(int adminMenuId)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);

        var userRoles = user != null
            ? await _userManager.GetRolesAsync(user)
            : new List<string>();

        var subMenus = await _context.AdminSubMenus
            .Where(x => x.IsActive && x.AdminMenuId == adminMenuId)
            .Include(x => x.NavigationTarget)
            .Include(x => x.AdminSubMenuPermissions)
            .OrderBy(x => x.Order)
            .Select(x => new AdminSubMenuViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Icon = x.Icon,
                Order = x.Order,
                OpenInNewTab = x.OpenInNewTab,
                AdminMenuId = x.AdminMenuId,

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

                AdminSubMenuPermissions = x.AdminSubMenuPermissions
                .Select(p => new AdminSubMenuPermissionViewModel
                {
                    Id = p.Id,
                    AdminSubMenuId = p.AdminSubMenuId,
                    RoleName = p.RoleName
                })
                .ToList(),

                IsVisible =
    userRoles.Contains("SuperAdmin") ||
    x.AdminSubMenuPermissions == null ||
    !x.AdminSubMenuPermissions.Any() ||
    x.AdminSubMenuPermissions.Any(p =>
        userRoles.Contains(p.RoleName))


            })
            .Where(x => x.IsVisible)
            .ToListAsync();

        return View(subMenus);
    }
}