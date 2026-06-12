using AnatoliaSmmPanel.Data.Models.Admin;
using AnatoliaSmmPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace AnatoliaSmmPanel.Data;

// ApplicationDbContext.Home.cs dosyası, ApplicationDbContext sınıfının HomeContext ile ilgili bölümlerini içerir.
public partial class ApplicationDbContext
{
    public DbSet<Menu> Menus { get; set; }
    public DbSet<MenuPermission> MenuPermissions { get; set; }

    public DbSet<AuthMenu> AuthMenus { get; set; }
    public DbSet<AuthMenuPermission> AuthMenuPermissions { get; set; }

    public DbSet<AdminMenu> AdminMenus { get; set; }
    public DbSet<AdminMenuPermission> AdminMenuPermissions { get; set; }

    public DbSet<AdminSubMenu> AdminSubMenus { get; set; }
    public DbSet<AdminSubMenuPermission> AdminSubMenuPermissions { get; set; }

    public DbSet<NavigationTarget> NavigationTargets { get; set; }


    partial void OnHomeModelCreating(ModelBuilder builder)
    {
        builder.Entity<AdminSubMenu>()
            .HasOne(x => x.NavigationTarget)
            .WithMany()
            .HasForeignKey(x => x.NavigationTargetId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
