using Wallet.Data.Models;

namespace Wallet.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class WalletDbContext : IdentityDbContext
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Asset>()
                .HasOne(c => c.Category)
                .WithMany(a => a.Assets)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }

        public DbSet<Asset> Assets { get; init; }

        public DbSet<Category> Categories { get; init; }
    }
}