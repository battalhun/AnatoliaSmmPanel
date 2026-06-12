using AnatoliaSmmPanel.Data;
using AnatoliaSmmPanel.Enums;
using AnatoliaSmmPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace AnatoliaSmmPanel.Seed
{
    public class MenuSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<HomeContext>();

            var menus = new List<Menu>
            {
                new Menu
                {
                    Name = "Home",
                    NavigationTarget = new NavigationTarget
                    {
                        // 🔥 Identity Login Page
                        Area = "Identity",
                        Page = "/Account/Login",
                        MenuConnect = MenuConnect.RazorPage
                    },
                    IsActive = true,
                    Order = 1,
                    OpenInNewTab = false
                },

                new Menu
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

                new Menu
                {
                    Name = "Sign Up",
                    NavigationTarget = new NavigationTarget
                    {
                        Area = "Identity",
                        Page = "/Account/Register",
                        MenuConnect = MenuConnect.RazorPage
                    },
                    IsActive = true,
                    Order = 3,
                    OpenInNewTab = false
                },

                new Menu
                {
                    Name = "Blog",
                    NavigationTarget = new NavigationTarget
                    {
                        Controller = "Blog",
                        Action = "Index",
                        MenuConnect = MenuConnect.Mvc
                    },
                    IsActive = true,
                    Order = 4,
                    OpenInNewTab = false
                }
            };

            foreach (var menu in menus)
            {
                bool exists = await context.Menus
                    .AnyAsync(x => x.Name == menu.Name);

                if (!exists)
                {
                    await context.Menus.AddAsync(menu);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}