using Wallet.Core.ViewModels.Transaction;
using Wallet.Infrastructure.Data.Models;

namespace Wallet.Core.Contracts
{
    public interface ITransactionService
    {
        (bool isDeposit, string error) Deposit(DepositModel model, string? identityName);
        (bool isWithdraw, string error) Withdraw(WithdrawModel model, string? identityName);
        WithdrawModel GetUserWithdrawModel(string user);
        List<UserTransactionsViewModel> GetUserTransactions(string username);
        Transaction CreateBuyTransaction(User user,decimal amount, decimal value);
    }
}
