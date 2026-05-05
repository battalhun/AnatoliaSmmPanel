using AnatoliaSmmPanel.Data;
using AnatoliaSmmPanel.Enums;
using AnatoliaSmmPanel.Models;
using Microsoft.EntityFrameworkCore;
using AnatoliaSmmPanel.Areas.Admin.Models;

public class AdminSubMenuSettingsSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<HomeContext>();

        //await context.Database.MigrateAsync();

        var parentMenu = await context.AdminMenus
            .FirstOrDefaultAsync(x => x.Name == "Settings");

        if (parentMenu == null)
            return;

        var subMenus = new List<(string Name, string Partial)>
        {
            ("General", "Settings/_GeneralSettings"),
            ("Providers", "Settings/_ProvidersSettings"),
            ("Payments", "Settings/_PaymentsSettings"),
            ("Modules", "Settings/_ModulesSettings"),
            ("Integrations", "Settings/_IntegrationsSettings"),
            ("Notifications", "Settings/_NotificationsSettings"),
            ("Bonuses", "Settings/_BonusesSettings"),
            ("Signup form", "Settings/_SignupFormSettings"),
            ("Ticket form", "Settings/_TicketFormSettings")
        };

        int order = 1;

        foreach (var item in subMenus)
        {
            var exists = await context.AdminSubMenus
                .AnyAsync(x => x.Name == item.Name && x.AdminMenuId == parentMenu.Id);

            if (!exists)
            {
                var subMenu = new AdminSubMenu
                {
                    Name = item.Name,
                    NavigationTarget = new NavigationTarget
                    {
                        Controller = null,
                        Action = null,
                        Page = null,
                        Area = null,
                        Url = null,

                        PartialView = item.Partial,

                        MenuConnect = (MenuConnect)3 // Partial
                    },
                    IsActive = true,
                    Order = order,

                    Icon = null,

                    OpenInNewTab = false,

                    AdminMenuId = parentMenu.Id
                };

                await context.AdminSubMenus.AddAsync(subMenu);
            }

            order++;
        }

        await context.SaveChangesAsync();
    }
}
