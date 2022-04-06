using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Wallet.Core.Constants;
using Wallet.Core.Contracts;
using Wallet.Infrastructure.Data.Models;

namespace Wallet.Test.ServicesTests
{
    using Xunit;

    public class RepositoryTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        [Fact]
        public void RepoGetAssetsTest()
        {
            var repo = GetInMemoryRepository();
            Seed(repo);

            var savedAssets = repo.All<Asset>();

            Assert.NotNull(savedAssets);
            Assert.Equal(3,savedAssets.Count());
        }

        [Fact]
        public void RepoDeleteTest()
        {
            var repo = GetInMemoryRepository();
            Seed(repo);

            var asset = repo.All<Asset>()
                .Where(a => a.Name == "a1")
                .FirstOrDefault();

            repo.Remove<Asset>(asset);
            repo.SaveChanges();

            Assert.Equal(2,repo.All<Asset>().Count());
        }

        [Fact]
        public void RepoAddTest()
        {
            var repo = GetInMemoryRepository();
            Seed(repo);

            var category = new Category()
            {
                Name = "c1",
                Description = ""
            };

            var asset1 = new Asset()
            {
                Name = "a1",
                Abbreviation = "a1",
                Category = category,
                Logo = new byte[2]
            };

            repo.Add<Category>(category);
            repo.Add<Asset>(asset1);
            repo.SaveChanges();

            Assert.Equal(2, repo.All<Category>().Count());
            Assert.Equal(4, repo.All<Asset>().Count());
        }

        private IRepository GetInMemoryRepository()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp=>dbContext.CreateContext())
                .AddSingleton<IRepository,Repository>()
                .BuildServiceProvider();

            return serviceProvider.GetService<IRepository>();
        }

        private void Seed(IRepository repo)
        {
            var category = new Category()
            {
                Name = "g1",
                Description = ""
            };

            var asset1 = new Asset()
            {
                Name = "a1",
                Abbreviation = "a1",
                Logo = new byte[5],
                Category = category
            };

            var asset2 = new Asset()
            {
                Name = "a2",
                Abbreviation = "a2",
                Logo = new byte[5],
                Category = category
            };

            var asset3 = new Asset()
            {
                Name = "a3",
                Abbreviation = "a3",
                Logo = new byte[6],
                Category = category
            };

            repo.Add<Asset>(asset1);
            repo.Add<Asset>(asset2);
            repo.Add<Asset>(asset3);
            repo.Add<Category>(category);
            repo.SaveChanges();
        }
    }

    
}
