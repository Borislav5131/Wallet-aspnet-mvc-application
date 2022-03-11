﻿using Wallet.Data.Models;

namespace Wallet.Infrastructure.Data
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
            base.OnModelCreating(builder);
        }

        public DbSet<Asset> Assets { get; set; }

        public DbSet<Category> Categories { get; set; }
    }
}