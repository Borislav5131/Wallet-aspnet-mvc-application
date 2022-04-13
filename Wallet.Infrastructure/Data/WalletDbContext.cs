using Microsoft.AspNetCore.Identity;
using Wallet.Infrastructure.Data.Models;
using Wallet.Infrastructure.InitialSeed;

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

            builder.Entity<UserAsset>()
                .Property(ua => ua.Quantity)
                .HasPrecision(12, 5);

            builder.ApplyConfiguration(new InitialDataConfiguration<Category>(@"../Wallet.Infrastructure/InitialSeed/categories.json"));
            builder.ApplyConfiguration(new InitialDataConfiguration<Asset>(@"../Wallet.Infrastructure/InitialSeed/assets.json"));
        }

        public DbSet<Asset> Assets { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Models.Wallet> Wallets { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<UserAsset> UserAssets { get; set; }
    }
}