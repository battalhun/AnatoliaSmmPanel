using AnatoliaSmmPanel.Enums;
using AnatoliaSmmPanel.Models;
using AnatoliaSmmPanel.Data.Models.Admin;
using Microsoft.EntityFrameworkCore;
using AnatoliaSmmPanel.Data;

namespace AnatoliaSmmPanel.Seed
{
    public static class AdminMenuSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {

            var _context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            //// Migration uygulanmış mı kontrol et
            //await context.Database.MigrateAsync();

            var menuNames = new List<string>
        {
            "Users",
            "Orders",
            "Subscriptions",
            "Dripfeed",
            "Refill",
            "Cancel",
            "Services",
            "Payments",
            "Tickets",
            "Affiliates",
            "Updates",
            "Reports",
            "Appearance",
            "Settings"
        };

            int order = 1;

            foreach (var menuName in menuNames)
            {
                // Aynı menü varsa tekrar ekleme
                bool exists = await _context.AdminMenus
                    .AnyAsync(x => x.Name == menuName);

                if (!exists)
                {
                    var actionName = menuName
                        .Replace("-", "")
                        .Replace(" ", "");

                    var menu = new AdminMenu
                    {
                        Name = menuName,
                        NavigationTarget = new NavigationTarget
                        {
                            Controller = menuName,
                            Action = "Index",
                            Page = null,
                            Area = null,
                            Url = null,
                            MenuConnect = MenuConnect.Mvc, // = 0
                        },

                        IsActive = true,
                        Order = order,

                        Icon = null,

                        IsAdminOnly = true,
                        OpenInNewTab = false
                    };

                    await _context.AdminMenus.AddAsync(menu);
                }

                order++;
            }

            await _context.SaveChangesAsync();
        }
    }
}