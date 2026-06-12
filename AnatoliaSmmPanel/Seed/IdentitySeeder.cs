using AnatoliaSmmPanel.Data.Models.Appliciton;
using AnatoliaSmmPanel.Models;
using Microsoft.AspNetCore.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var roles = new[]
        {
            "SuperAdmin", "Admin",
            "AdminUsers", "AdminOrders", "AdminSubscriptions",
            "AdminDripFeed", "AdminRefill", "AdminCancel",
            "AdminServices", "AdminPayments", "AdminTickets",
            "AdminAffiliates", "AdminUpdates", "AdminReports",
            "AdminAppearance", "AdminSettings", "AdminRoles", "AdminMenu"
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        var email = "admin@canbazmedia.tr";
        var password = "Admin123!";

        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(user, password);

            if (!createResult.Succeeded)
                throw new Exception(string.Join(", ", createResult.Errors.Select(e => e.Description)));
        }

        user = await userManager.FindByEmailAsync(email);

        foreach (var role in roles)
        {
            if (!await userManager.IsInRoleAsync(user, role))
            {
                var result = await userManager.AddToRoleAsync(user, role);
                if (!result.Succeeded)
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}