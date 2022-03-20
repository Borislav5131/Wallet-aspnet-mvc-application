using Wallet.Core.Constants;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels.Transaction;
using Wallet.Infrastructure.Data;
using Wallet.Infrastructure.Data.Models;

namespace Wallet.Core.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IRepository _repo;
        private readonly IUserService _userService;

        public TransactionService(IRepository repo,
            IUserService userService)
        {
            _repo = repo;
            _userService = userService;
        }

        public WithdrawModel GetUserWithdrawModel(string userName)
        {
            var user = _userService.GetUserByName(userName);

            if (user == null)
            {
                return null;
            }

            return new WithdrawModel()
            {
                UserBalance = user.Balance
            };
        }

        public (bool isDeposit, string error) Deposit(DepositModel model, string? identityName)
        {
            bool isDeposit = false;
            string error = String.Empty;

            var user = _userService.GetUserByName(identityName);

            if (user == null)
            {
                return (isDeposit, error = "The transaction cannot be completed!");
            }

            Transaction transaction = new Transaction()
            {
                User = user,
                Value = model.Value,
                Date = DateTime.UtcNow,
                Type = TransactionTypes.Deposit
            };

            try
            {
                user.Balance += model.Value;
                user.Transactions.Add(transaction);
                _repo.Add<Transaction>(transaction);
                _repo.SaveChanges();
                isDeposit = true;
            }
            catch (Exception)
            {
                error = "The transaction cannot be completed!";
            }

            return (isDeposit, error);
        }

        public (bool isWithdraw, string error) Withdraw(WithdrawModel model, string? identityName)
        {
            bool isDeposit = false;
            string error = String.Empty;

            var user = _userService.GetUserByName(identityName);

            if (user == null || user.Balance - model.Value < 0)
            {
                return (isDeposit, error = "The transaction cannot be completed!");
            }

            Transaction transaction = new Transaction()
            {
                User = user,
                Value = model.Value,
                Date = DateTime.UtcNow,
                Type = TransactionTypes.Withdraw
            };

            try
            {
                user.Balance -= model.Value;
                user.Transactions.Add(transaction);
                _repo.Add<Transaction>(transaction);
                _repo.SaveChanges();
                isDeposit = true;
            }
            catch (Exception)
            {
                error = "The transaction cannot be completed!";
            }

            return (isDeposit, error);
        }

        
    }
}
