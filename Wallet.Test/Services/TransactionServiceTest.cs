using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Wallet.Core.Constants;
using Wallet.Core.Contracts;
using Wallet.Core.Services;
using Wallet.Core.ViewModels.Transaction;
using Wallet.Infrastructure.Data;
using Wallet.Infrastructure.Data.Models;
using Xunit;

namespace Wallet.Test.Services
{
    public class TransactionServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;
        private IRepository repo;
        private ITransactionService transactionService;

        [Fact]
        public void GetUserTransactionsShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var user = repo.All<User>().First();

            Assert.NotNull(transactionService.GetUserTransactions(user.UserName));
            Assert.NotEmpty(transactionService.GetUserTransactions(user.UserName));
            Assert.IsType<List<UserTransactionsViewModel>>(transactionService.GetUserTransactions(user.UserName));
        }

        [Fact]
        public void CreateBuyTransactionShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var user = repo.All<User>().First();

            Assert.NotNull(transactionService.CreateBuyTransaction(user,50,84));
            Assert.IsType<Transaction>(transactionService.CreateBuyTransaction(user, 46, 65));
        }

        [Fact]
        public void CreateSellTransactionShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var user = repo.All<User>().First();

            Assert.NotNull(transactionService.CreateSellTransaction(user, 50, 84));
            Assert.IsType<Transaction>(transactionService.CreateSellTransaction(user, 46, 65));
        }

        [Fact]
        public void GetUserWithdrawModelShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var user = repo.All<User>().First();

            Assert.IsType<WithdrawModel>(transactionService.GetUserWithdrawModel(user.UserName));
            Assert.NotNull(transactionService.GetUserWithdrawModel(user.UserName));
            Assert.Null(transactionService.GetUserWithdrawModel("Invalid user"));
        }

        [Fact]
        public void DepositShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var user = repo.All<User>().First();

            var modelCorrect = new DepositModel()
            {
                Value = 50
            };

            Assert.Equal((false, "The transaction cannot be completed!"),transactionService.Deposit(modelCorrect,"Invalid user"));
            Assert.Equal((true,String.Empty),transactionService.Deposit(modelCorrect,user.UserName));
        }


        [Fact]
        public void WithdrawShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var modelCorrect = new WithdrawModel()
            {
                Value = 50,
                UserBalance = 500
            };

            Assert.Equal((false, "The transaction cannot be completed!"), transactionService.Withdraw(modelCorrect, "Invalid user"));
            Assert.Equal((false, "The transaction cannot be completed!"), transactionService.Withdraw(modelCorrect, "User"));
            Assert.Equal((true, String.Empty), transactionService.Withdraw(modelCorrect, "test"));
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
            var userService = new UserService(repo, userManager,roleManager);
            transactionService = new TransactionService(repo, userService);
        }

        private void Seed(IRepository repo)
        {
            var user = new User()
            {
                UserName = "test",
                Email = "test@abv.bg",
                Image = new byte[5]
            };
            user.Wallet.Balance = 1000;

            var userLowBalance = new User()
            {
                UserName = "User",
                Email = "user@abv.bg",
                Image = new byte[60],
            };
            userLowBalance.Wallet.Balance = 10;

            var transaction = new Transaction()
            {
                User = user,
                Date = DateTime.UtcNow,
                Value = 55,
                Amount = 65,
                Type = TransactionTypes.Buy
            };

            user.Wallet.Transactions.Add(transaction);

            repo.Add<User>(user);
            repo.Add<User>(userLowBalance);
            repo.Add<Transaction>(transaction);
            repo.SaveChanges();
        }
    }
}
