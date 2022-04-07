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

        public TransactionService(
            IRepository repo,
            IUserService userService)
        {
            _repo = repo;
            _userService = userService;
        }

        public List<UserTransactionsViewModel> GetUserTransactions(string username)
            => _repo.All<Transaction>()
                .Where(t=>t.User.UserName == username)
                .OrderByDescending(t => t.Date)
                .Select(t=> new UserTransactionsViewModel()
                {
                    Username = t.User.UserName,
                    Date = t.Date.ToString("dd/MM/yyyy HH:mm:ss"),
                    Type = t.Type.ToString(),
                    Value = t.Value,
                    Amont = t.Amount
                })
                .ToList();

        public Transaction CreateBuyTransaction(User user,decimal amount,decimal value)
            => new Transaction()
            {
                User = user,
                Date = DateTime.UtcNow,
                Value = value,
                Amount = amount,
                Type = TransactionTypes.Buy,
            };

        public Transaction CreateSellTransaction(User user, decimal amount, decimal value)
            => new Transaction()
            {
                User = user,
                Date = DateTime.UtcNow,
                Value = value,
                Amount = amount,
                Type = TransactionTypes.Sell,
            };

        public WithdrawModel GetUserWithdrawModel(string userName)
        {
            var user = _userService.GetUserByName(userName);

            if (user == null)
            {
                return null;
            }

            return new WithdrawModel()
            {
                UserBalance = user.Wallet.Balance
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
                user.Wallet.Balance += model.Value;
                user.Wallet.Transactions.Add(transaction);
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

            if (user == null || user.Wallet.Balance - model.Value < 0)
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
                user.Wallet.Balance -= model.Value;
                user.Wallet.Transactions.Add(transaction);
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

        public bool ClearTransactionsOfUser(User user)
        {
            user.Wallet.Transactions.Clear();

            var transactions = _repo.All<Transaction>().Where(t => t.User == user).ToList();

            try
            {
                foreach (var transaction in transactions)
                {
                    _repo.Remove<Transaction>(transaction);
                }

                _repo.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
