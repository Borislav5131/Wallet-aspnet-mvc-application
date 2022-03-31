using Wallet.Infrastructure.Data.Models;

namespace Wallet.Infrastructure.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class WalletDbContext : IdentityDbContext<User>
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Models.User>()
                .HasOne(u => u.Wallet)
                .WithOne(w => w.User)
                .HasForeignKey<Models.Wallet>(w=>w.UserId);
        }

        public DbSet<Asset> Assets { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Models.Wallet> Wallets { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<UserAsset> UserAssets { get; set; }
    }
}