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

        public TransactionService(IRepository repo)
        {
            _repo = repo;
        }

        public (bool isDeposit, string error) Deposit(DepositModel model, string? identityName)
        {
            bool isDeposit = false;
            string error = String.Empty;

            var user = _repo.All<User>()
                .FirstOrDefault(x => x.UserName == identityName);

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
    }
}
