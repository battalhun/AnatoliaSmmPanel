using AnatoliaSmmPanel.Data.Models.Appliciton;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnatoliaSmmPanel.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options
        ) : base(options)
        {
        }

        // Partial metot tanımlamaları (diğer dosyalarda gövdesi yazılacak)
        partial void OnAdminModelCreating(ModelBuilder builder);
        partial void OnHomeModelCreating(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(u => u.Id);

                // 1. Anahtar olarak tanımlıyoruz
                entity.HasAlternateKey(u => u.UserId);

                // 2. SADECE otomatik artan olduğunu söylüyoruz. (Sorun çıkartan Ignore satırını sildik!)
                entity.Property(u => u.UserId)
                      .ValueGeneratedOnAdd();

                entity.Property(u => u.Nickname).HasMaxLength(100);
                entity.Property(u => u.Status).HasMaxLength(50);
            });

            // Identity Tablo İsimlendirmeleri
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityRole>(entity =>
            {
                entity.ToTable("Roles");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            // Admin ve Home mappinglerini tetikle
            OnAdminModelCreating(builder);
            OnHomeModelCreating(builder);
        }
    }
}