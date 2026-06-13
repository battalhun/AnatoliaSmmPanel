using AnatoliaSmmPanel.Data;
using AnatoliaSmmPanel.Enums;
using AnatoliaSmmPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace AnatoliaSmmPanel.Seed
{
    public class AuthMenuSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var _context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            var authMenus = new List<AuthMenu>
            {
                new AuthMenu
                {
                    Name = "New Order",
                    NavigationTarget = new NavigationTarget
                    {
                        Controller = "Home",
                        Action = "NewOrder",
                        MenuConnect = MenuConnect.Mvc
                    },
                    IsActive = true,
                    Order = 1,
                    OpenInNewTab = false
                },

                new AuthMenu
                {
                    Name = "Services",
                    NavigationTarget = new NavigationTarget
                    {
                        Controller = "Home",
                        Action = "Services",
                        MenuConnect = MenuConnect.Mvc
                    },
                    IsActive = true,
                    Order = 2,
                    OpenInNewTab = false
                },
                 new AuthMenu
                {
                    Name = "Tickets",
                    NavigationTarget = new NavigationTarget
                    {
                        Controller = "Home",
                        Action = "Tickets",
                        MenuConnect = MenuConnect.Mvc
                    },
                    IsActive = true,
                    Order = 2,
                    OpenInNewTab = false
                }

            };

            foreach (var menu in authMenus)
            {
                bool exists = await _context.AuthMenus
                    .AnyAsync(x => x.Name == menu.Name);

                if (!exists)
                {
                    await _context.AuthMenus.AddAsync(menu);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
