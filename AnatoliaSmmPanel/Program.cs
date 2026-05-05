using AnatoliaSmmPanel.Data;
using AnatoliaSmmPanel.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AnatoliaSmmPanel.Services;
using AnatoliaSmmPanel.Areas.Admin.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDbContext<HomeContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddHttpClient<ISmmApiService, SmmApiService>();
builder.Services.AddSingleton<ISettingsService, SettingsService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Identity roles + admin user
    await IdentitySeeder.SeedAsync(services);

    // Admin panel menu seed
    await AdminMenuSeeder.SeedAsync(services);

    // Admin panel Settings submenu seed
    await AdminSubMenuSettingsSeeder.SeedAsync(services);

    await MenuSeeder.SeedAsync(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "MyAreas",
    pattern: "{area:exists}/{controller=Users}/{action=Index}/{id?}");

app.MapGet("/", context =>
{
    if (context.User?.Identity?.IsAuthenticated == true)
    {
        //context.Response.Redirect("/new-order");
        context.Response.Redirect("/admin/settings");
    }
    else
    {
        context.Response.Redirect("/signin");

    }

    return Task.CompletedTask;
});

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
