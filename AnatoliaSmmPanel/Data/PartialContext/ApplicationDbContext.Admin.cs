using AnatoliaSmmPanel.Data.Models.Admin;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AnatoliaSmmPanel.Data
{

    // ApplicationDbContext.Admin.cs dosyası, ApplicationDbContext sınıfının AdminContext ile ilgili bölümlerini içerir.
    public partial class ApplicationDbContext
    {
        public DbSet<Provider> Providers { get; set; }
        public DbSet<SmmService> SmmServices { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<ExternalServiceInfo> ExternalServiceInfos { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketMessage> TicketMessages { get; set; }

        partial void OnAdminModelCreating(ModelBuilder builder)
        {
            builder.Entity<Order>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasOne(o => o.User)
                      .WithMany()
                      .HasForeignKey(o => o.UserId)     // Orders tablosundaki int UserId sütunu
                      .HasPrincipalKey(u => u.UserId)   // Users tablosundaki int UserId sütunu (Burayı güncelledik!)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_Orders_Users_UserId");
            });
        }
    }
}