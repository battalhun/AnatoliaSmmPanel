using AnatoliaSmmPanel.Data;
using AnatoliaSmmPanel.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnatoliaSmmPanel.Areas.Admin.ViewModels;

public class AdminMenuViewComponent : ViewComponent
{
    private readonly HomeContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public AdminMenuViewComponent(
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

        if (user != null)
        {
            userRoles = (await _userManager.GetRolesAsync(user)).ToList();
        }

        var adminmenuss = await _context.AdminMenus
            .Where(x => x.IsActive)
            .Include(x => x.NavigationTarget)
            .Include(x => x.AdminMenuPermissions)
            .OrderBy(x => x.Order)
            .Select(x => new AdminMenuViewModel
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


                AdminMenuPermissions = x.AdminMenuPermissions
                    .Select(p => new AdminMenuPermissionViewModel
                    {
                        Id = p.Id,
                        AdminMenuId = p.AdminMenuId,
                        RoleName = p.RoleName
                    })
                    .ToList(),

                IsVisible =
    userRoles.Contains("SuperAdmin") ||
    x.AdminMenuPermissions == null ||
    !x.AdminMenuPermissions.Any() ||
    x.AdminMenuPermissions.Any(p =>
        userRoles.Contains(p.RoleName))

            })
            .Where(x => x.IsVisible)
            .ToListAsync();

        return View(adminmenuss);
    }
}