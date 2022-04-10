using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Wallet.Core.Constants;
using Wallet.Core.Contracts;
using Wallet.Core.Services;
using Wallet.Core.ViewModels.User;
using Wallet.Infrastructure.Data.Models;
using Xunit;

namespace Wallet.Test.Services
{
    public class UserServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;
        private IRepository repo;
        private IUserService userService;

        [Fact]
        public async void GetUsersShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            Assert.NotEmpty(await userService.GetUsers());
            Assert.NotNull(await userService.GetUsers());
            Assert.IsType<List<UserListViewModel>>(await userService.GetUsers());
        }

        [Fact]
        public void GetUserByNameShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            Assert.NotNull(userService.GetUserByName("test"));
            Assert.NotNull(userService.GetUserByName("test2"));
            Assert.Null(userService.GetUserByName("Invalid name"));
            Assert.IsType<User>(userService.GetUserByName("test"));
        }

        [Fact]
        public void GetUserByIdShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var user1 = repo.All<User>().Where(u => u.UserName == "test").First();
            var user2 = repo.All<User>().Where(u => u.UserName == "test2").First();

            Assert.NotNull(userService.GetUserById(user2.Id));
            Assert.NotNull(userService.GetUserById(user1.Id));
            Assert.Null(userService.GetUserById(Guid.NewGuid().ToString()));
            Assert.IsType<User>(userService.GetUserById(user1.Id));
        }

        [Fact]
        public void GetDetailsOfUserShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var user1 = repo.All<User>().Where(u => u.UserName == "test").First();
            var user2 = repo.All<User>().Where(u => u.UserName == "test2").First();

            Assert.NotNull(userService.GetDetailsOfUser(user1.Id));
            Assert.NotNull(userService.GetDetailsOfUser(user2.Id));
            Assert.IsType<EditUserModel>(userService.GetDetailsOfUser(user1.Id));
            Assert.IsType<EditUserModel>(userService.GetDetailsOfUser(user2.Id));
            Assert.Null(userService.GetDetailsOfUser("ssucscjs"));
        }

        [Fact]
        public void GetUserInformationShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var user1 = repo.All<User>().Where(u => u.UserName == "test").First();
            var user2 = repo.All<User>().Where(u => u.UserName == "test2").First();

            Assert.NotNull(userService.GetUserInformation(user1.UserName));
            Assert.NotNull(userService.GetUserInformation(user2.UserName));
            Assert.IsType<UserViewModel>(userService.GetUserInformation(user1.UserName));
            Assert.IsType<UserViewModel>(userService.GetUserInformation(user2.UserName));
            Assert.Null(userService.GetUserInformation("ssucscjs"));
        }

        [Fact]
        public void GetUserBalanceShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var user1 = repo.All<User>().Where(u => u.UserName == "test").First();
            var user2 = repo.All<User>().Where(u => u.UserName == "test2").First();

            Assert.IsType<decimal>(userService.GetUserBalance(user1.UserName));
            Assert.IsType<decimal>(userService.GetUserBalance(user2.UserName));
            Assert.Equal(0, userService.GetUserBalance(user1.UserName));
            Assert.Equal(1000, userService.GetUserBalance(user2.UserName));
        }

        [Fact]
        public void DeleteShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var user = repo.All<User>().Where(u => u.UserName == "test").First();

            Assert.False(userService.Delete("kiidkd"));
            Assert.True(userService.Delete(user.Id));
        }

        [Fact]
        public async void EditShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var user = repo.All<User>().Where(u => u.UserName == "test").First();

            var modelCorrect = new EditUserModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Image = user.Image.ToString(),
            };

            var modelInCorrect = new EditUserModel()
            {
                Id = "user.Id",
                UserName = "user.UserName",
                Email = "user.Email",
                Image = "user.Image.ToString()",
            };

            Assert.Equal((false, "Invalid operation"), await userService.Edit(modelInCorrect, new byte[5]));
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
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            userService = new UserService(repo, userManager,roleManager);
        }

        private void Seed(IRepository repo)
        {
            var user = new User()
            {
                UserName = "test",
                Email = "test@abv.bg",
                Image = new byte[5]
            };

            var user2 = new User()
            {
                UserName = "test2",
                Email = "test2@abv.bg",
                Image = new byte[58]
            };
            user2.Wallet.Balance += 1000;

            repo.Add<User>(user);
            repo.Add<User>(user2);
            repo.SaveChanges();
        }
    }
}
