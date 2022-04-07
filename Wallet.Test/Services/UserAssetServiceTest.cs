using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Wallet.Core.Constants;
using Wallet.Core.Contracts;
using Wallet.Core.Services;
using Wallet.Core.ViewModels.UserAsset;
using Wallet.Infrastructure.Data.Models;
using Xunit;

namespace Wallet.Test.Services
{
    public class UserAssetServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;
        private IRepository repo;
        private IUserAssetService userAssetService;

        [Fact]
        public void GetUserAssetsInformationShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var user = repo.All<User>().First();

            Assert.IsType<List<UserAssetViewModel>>(userAssetService.GetUserAssetsInformation(user.UserName));
            Assert.NotNull(userAssetService.GetUserAssetsInformation(user.UserName));
            Assert.NotEmpty(userAssetService.GetUserAssetsInformation(user.UserName));
        }

        private void GetInMemoryRepository()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IRepository, Repository>()
                .BuildServiceProvider();

            repo = serviceProvider.GetService<IRepository>();
            var userManager = serviceProvider.GetService<UserManager<User>>();
            var userService = new UserService(repo, userManager);
            var transactionService = new TransactionService(repo, userService);
            userAssetService = new UserAssetService(repo, userService, transactionService);
        }

        private void Seed(IRepository repo)
        {
            var user = new User()
            {
                UserName = "test",
                Email = "test@abv.bg",
                Image = new byte[5]
            };

            var userAsset = new UserAsset()
            {
                Name = "a1",
                Abbreviation = "a1",
                Logo = String.Empty,
                CategoryName = "Stock",
                BuyedPrice = 965,
                Amount = 939,
                Quantity = 0.857m
            };

            user.Wallet.UserAssets.Add(userAsset);

            repo.Add<User>(user);
            repo.Add<UserAsset>(userAsset);
            repo.SaveChanges();
        }
    }
}
