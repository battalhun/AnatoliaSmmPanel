using Microsoft.AspNetCore.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var roles = new[]
{
    "SuperAdmin",
    "Admin",

    "AdminUsers",
    "AdminOrders",
    "AdminSubscriptions",
    "AdminDripFeed",
    "AdminRefill",
    "AdminCancel",
    "AdminServices",
    "AdminPayments",
    "AdminTickets",
    "AdminAffiliates",
    "AdminUpdates",
    "AdminReports",
    "AdminAppearance",
    "AdminSettings",
    "AdminRoles",
    "AdminMenu"
};

        // 1. Roles
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // 2. User
        var email = "admin@canbazmedia.tr";
        var password = "Admin123!";

        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(user, password);

            if (!createResult.Succeeded)
                throw new Exception(string.Join(", ", createResult.Errors.Select(e => e.Description)));
        }

        // 3. FORCE reload
        user = await userManager.FindByEmailAsync(email);

        // 4. Roles attach
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