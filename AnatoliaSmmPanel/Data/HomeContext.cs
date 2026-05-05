using AnatoliaSmmPanel.Areas.Admin.Models;
using AnatoliaSmmPanel.Models;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace AnatoliaSmmPanel.Data
{
    public class HomeContext : DbContext
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

        public DbSet<Provider> Providers { get; set; }

        public HomeContext(DbContextOptions<HomeContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // AdminSubMenu -> NavigationTarget
            modelBuilder.Entity<AdminSubMenu>()
                .HasOne(x => x.NavigationTarget)
                .WithMany()
                .HasForeignKey(x => x.NavigationTargetId)
                .OnDelete(DeleteBehavior.Restrict);

            // AdminMenu -> NavigationTarget
            modelBuilder.Entity<AdminMenu>()
                .HasOne(x => x.NavigationTarget)
                .WithMany()
                .HasForeignKey(x => x.NavigationTargetId)
                .OnDelete(DeleteBehavior.Restrict);

            // Permission -> SubMenu
            modelBuilder.Entity<AdminSubMenuPermission>()
                .HasOne(x => x.AdminSubMenu)
                .WithMany()
                .HasForeignKey(x => x.AdminSubMenuId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}