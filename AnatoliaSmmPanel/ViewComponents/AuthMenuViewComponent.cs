    using AnatoliaSmmPanel.Data;
    using AnatoliaSmmPanel.Data.Models.Appliciton;
    using AnatoliaSmmPanel.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    namespace AnatoliaSmmPanel.ViewComponents;

    public class AuthMenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthMenuViewComponent(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
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


            var authMenus = await _context.AuthMenus
                .Where(x => x.IsActive)
                .Include(x => x.NavigationTarget)
                .Include(x => x.authMenuPermissions)
                .OrderBy(x => x.Order)
                .Select(x => new AuthMenuViewModel
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
                        x.authMenuPermissions == null ||
                        !x.authMenuPermissions.Any() ||
                        x.authMenuPermissions.Any(p => userRoles.Contains(p.RoleName))
                })
                .Where(x => x.IsVisible)
                .ToListAsync();

            return View(authMenus);
        }
    }
