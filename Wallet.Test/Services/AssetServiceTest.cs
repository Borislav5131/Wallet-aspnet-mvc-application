using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Wallet.Core.Constants;
using Wallet.Core.Contracts;
using Wallet.Core.Services;
using Wallet.Core.ViewModels.Asset;
using Wallet.Infrastructure.Data.Models;
using Xunit;

namespace Wallet.Test.Services
{
    public class AssetServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;
        private IRepository repo;
        private IAssetService assetService;

        [Fact]
        public void GetAllAssetsShouldReturnListOfAsset()
        {
            GetInMemoryRepository();
            Seed(repo);

            var assets = assetService.GetAllAssets();

            Assert.NotNull(assets);
            Assert.Equal(3, assets.Count);
        }

        [Fact]
        public void GetAssetByIdShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var assets = assetService.GetAllAssets();
            var assetId = assets[0].AssetId;

            var fakeAssetId = Guid.NewGuid();

            Assert.NotNull(assetService.GetAssetById(assetId));
            Assert.Null(assetService.GetAssetById(fakeAssetId));
        }

        [Fact]
        public void GetAssetsInCategoryShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var categoryId = repo.All<Category>().Where(x => x.Name == "c1").Select(x => x.Id).First();
            var assetsFromCategoryFromRepo = repo.All<Asset>().Where(a => a.Category.Id == categoryId).ToList();

            Assert.NotNull(assetService.GetAssetsInCategory(categoryId));
            Assert.Equal(assetsFromCategoryFromRepo.Count, assetService.GetAssetsInCategory(categoryId).Count);
        }

        [Fact]
        public void CreateAssetShouldReturnCorrectInAnyCases()
        {
            GetInMemoryRepository();
            Seed(repo);

            var asset = repo.All<Asset>().First();

            var modelExist = new CreateAssetModel()
            {
                Abbreviation = asset.Abbreviation,
                CategoryName = asset.Category.Name,
                Name = asset.Name,
                Value = asset.Value
            };

            var modelCategoryNotExist = new CreateAssetModel()
            {
                Abbreviation = "a5",
                CategoryId = Guid.NewGuid(),
                CategoryName = "c5",
                Name = "a5",
            };

            var correctModel = new CreateAssetModel()
            {
                Abbreviation = "a5",
                CategoryName = asset.Category.Name,
                CategoryId = asset.CategoryId,
                Name = "a5",
                Value = 500
            };

            Assert.Equal((false, "Asset exist!"), assetService.Create(modelExist, new byte[5]));
            Assert.Equal((false, "Category is not valid!"), assetService.Create(modelCategoryNotExist, new byte[9]));
            Assert.Equal((true,null), assetService.Create(correctModel,new byte[3]));
        }

        [Fact]
        public void EditAssetShouldReturnCorrectInAnyCases()
        {
            GetInMemoryRepository();
            Seed(repo);

            var modelNotCorrect = new EditAssetModel()
            {
                Abbreviation = "a5",
                CategoryId = Guid.NewGuid(),
                Category = "c5",
                Name = "a5",
            };

            var asset = repo.All<Asset>().First();

            var modelNotExistCategory = new EditAssetModel()
            {
                Abbreviation = asset.Abbreviation,
                Name = asset.Name,
                AssetId = asset.Id,
                Category = "c7",
            };

            var modelCorrect = new EditAssetModel()
            {
                Abbreviation = "a7",
                Name = "a7",
                Category = asset.Category.Name,
                CategoryId = asset.CategoryId,
                AssetId = asset.Id,
            };

            Assert.Equal((false, "Invalid operation"),assetService.Edit(modelNotCorrect,new byte[2]));
            Assert.Equal((false, "Invalid operation"),assetService.Edit(modelNotExistCategory,new byte[4]));
            Assert.Equal((true,null),assetService.Edit(modelCorrect,new byte[508]));
            Assert.Equal((false, "Logo must be max 2 MB"),assetService.Edit(modelCorrect,new byte[3 * 1024 * 1024]));
        }

        [Fact]
        public void BuyAssetShouldReturnCorrectInAnyCases()
        {
            GetInMemoryRepository();
            Seed(repo);

            var modelNotCorrect = new BuyAssetModel()
            {
                Name = "a4"
            };

            var asset = repo.All<Asset>().First();
            var user = repo.All<User>().First();

            var modelNotEnoughBalance = new BuyAssetModel()
            {
                AssetId = asset.Id,
                Name = asset.Name,
                Abbreviation = asset.Abbreviation,
                CategoryName = asset.Category.Name,
                Value = 50,
                Quantity = 9,
                Amount = 67,
            };

            var modelCorrect = new BuyAssetModel()
            {
                AssetId = asset.Id,
                Name = asset.Name,
                Abbreviation = asset.Abbreviation,
                CategoryName = asset.Category.Name,
                Value = 0,
                Quantity = 5,
                Amount = 0,
                Logo = ""
            };

            Assert.Equal((false, "Not found!"), assetService.BuyAsset(modelNotCorrect,"hi"));
            Assert.Equal((false, "Not enough money!"),assetService.BuyAsset(modelNotEnoughBalance,user.UserName));
            Assert.Equal((true,string.Empty),assetService.BuyAsset(modelCorrect,user.UserName));
        }

        [Fact]
        public void DeleteAssetShouldReturnCorrectInAnyCases()
        {
            GetInMemoryRepository();
            Seed(repo);

            var asset = repo.All<Asset>().First();

            Assert.True(assetService.Delete(asset.Id));

            asset.CategoryId = Guid.NewGuid();

            Assert.False(assetService.Delete(Guid.NewGuid()));
            Assert.False(assetService.Delete(asset.Id));
        }

        [Fact]
        public void GetDetailsOfAssetShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var asset = repo.All<Asset>().First();

            Assert.IsType<EditAssetModel>(assetService.GetDetailsOfAsset(asset.Id));
        }

        [Fact]
        public void GetBuyInformationOfAsset()
        {
            GetInMemoryRepository();
            Seed(repo);

            var asset = repo.All<Asset>().First();
            var user = repo.All<User>().First();

            Assert.IsType<BuyAssetModel>(assetService.GetBuyInformationOfAsset(asset.Id, user.UserName));
        }

        private void GetInMemoryRepository()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IRepository, Repository>()
                .AddMemoryCache()
                .BuildServiceProvider();

            repo = serviceProvider.GetService<IRepository>();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            var userManager = serviceProvider.GetService<UserManager<User>>();
            var categoryService = new CategoryService(repo, memoryCache);
            var userService = new UserService(repo, userManager);
            var transactionService = new TransactionService(repo, userService);
            assetService = new AssetService(repo, memoryCache, userService, categoryService, transactionService);
        }

        private void Seed(IRepository repo)
        {
            var category = new Category()
            {
                Name = "c1",
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

            var user = new User()
            {
                UserName = "test",
                Email = "test@abv.bg",
                Image = new byte[5]
            };

            repo.Add<User>(user);
            repo.Add<Asset>(asset1);
            repo.Add<Asset>(asset2);
            repo.Add<Asset>(asset3);
            repo.Add<Category>(category);
            repo.SaveChanges();
        }
    }
}
