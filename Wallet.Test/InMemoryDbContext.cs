using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Wallet.Infrastructure.Data;

namespace Wallet.Test
{
    public class InMemoryDbContext
    {
        private readonly SqliteConnection connection;
        private readonly DbContextOptions<WalletDbContext> dbContextOptions;

        public InMemoryDbContext()
        {
            connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            dbContextOptions = new DbContextOptionsBuilder<WalletDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new WalletDbContext(dbContextOptions);

            context.Database.EnsureCreated();
        }

        public WalletDbContext CreateContext() => new WalletDbContext(dbContextOptions);

        public void Dispose() => connection.Dispose();
    }
}
