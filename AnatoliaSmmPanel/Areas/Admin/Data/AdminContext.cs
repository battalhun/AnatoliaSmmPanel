using AnatoliaSmmPanel.Areas.Admin.Models;
using AnatoliaSmmPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace AnatoliaSmmPanel.Areas.Admin.Data;

public class AdminContext : DbContext
{
    public DbSet<Provider> Providers { get; set; }
    public DbSet<SmmService> SmmServices { get; set; }
    public DbSet<ServiceCategory> ServiceCategories { get; set; }
    public DbSet<ExternalServiceInfo> externalServiceInfos { get; set; }

    public AdminContext(DbContextOptions<AdminContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    //    base.OnModelCreating(modelBuilder);

    //    // =========================
    //    // AdminSubMenu -> NavigationTarget
    //    // =========================
    //    modelBuilder.Entity<AdminSubMenu>()
    //        .HasOne(x => x.NavigationTarget)
    //        .WithMany()
    //        .HasForeignKey(x => x.NavigationTargetId)
    //        .OnDelete(DeleteBehavior.Restrict);

    //    // =========================
    //    // AdminMenu -> NavigationTarget
    //    // =========================
    //    modelBuilder.Entity<AdminMenu>()
    //        .HasOne(x => x.NavigationTarget)
    //        .WithMany()
    //        .HasForeignKey(x => x.NavigationTargetId)
    //        .OnDelete(DeleteBehavior.Restrict);

    //    // =========================
    //    // AdminSubMenuPermission -> AdminSubMenu
    //    // =========================
    //    modelBuilder.Entity<AdminSubMenuPermission>()
    //        .HasOne(x => x.AdminSubMenu)
    //        .WithMany()
    //        .HasForeignKey(x => x.AdminSubMenuId)
    //        .OnDelete(DeleteBehavior.Restrict);

    //    // =========================
    //    // SmmService -> Provider
    //    // =========================
    //    modelBuilder.Entity<SmmService>()
    //        .HasOne(x => x.Provider)
    //        .WithMany(x => x.Services)
    //        .HasForeignKey(x => x.ProviderId)
    //        .OnDelete(DeleteBehavior.Restrict);

    //    // =========================
    //    // SmmService -> Category
    //    // =========================
    //    modelBuilder.Entity<SmmService>()
    //        .HasOne(x => x.serviceCategory)
    //        .WithMany(x => x.Services)
    //        .HasForeignKey(x => x.CategoryId)
    //        .OnDelete(DeleteBehavior.Restrict);

    //    // =========================
    //    // SmmService optimizations
    //    // =========================
    //    modelBuilder.Entity<SmmService>()
    //        .Property(x => x.Rate)
    //        .HasColumnType("decimal(18,4)");

    //    modelBuilder.Entity<SmmService>()
    //        .HasIndex(x => new { x.ProviderId, x.ExternalServiceId })
    //        .IsUnique();
    }
}