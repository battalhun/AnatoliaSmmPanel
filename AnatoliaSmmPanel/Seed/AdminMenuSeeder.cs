using AnatoliaSmmPanel.Data;
using AnatoliaSmmPanel.Enums;
using AnatoliaSmmPanel.Models;
using Microsoft.EntityFrameworkCore;
using AnatoliaSmmPanel.Areas.Admin.Models;

public static class AdminMenuSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<HomeContext>();

        //// Migration uygulanmış mı kontrol et
        //await context.Database.MigrateAsync();

        var menuNames = new List<string>
        {
            "Users",
            "Orders",
            "Subscriptions",
            "Drip-feed",
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
            bool exists = await context.AdminMenus
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
                        Controller = "Admin",
                        Action = actionName,
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

                await context.AdminMenus.AddAsync(menu);
            }

            order++;
        }

        await context.SaveChangesAsync();
    }
}