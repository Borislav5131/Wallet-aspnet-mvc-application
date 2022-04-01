using Wallet.Core.ViewModels.Transaction;
using Wallet.Infrastructure.Data.Models;

namespace Wallet.Core.Contracts
{
    public interface ITransactionService
    {
        List<UserTransactionsViewModel> GetUserTransactions(string username);
        Transaction CreateBuyTransaction(User user, decimal amount, decimal value);
        Transaction CreateSellTransaction(User user, decimal amount, decimal value);
        WithdrawModel GetUserWithdrawModel(string user);
        (bool isDeposit, string error) Deposit(DepositModel model, string? identityName);
        (bool isWithdraw, string error) Withdraw(WithdrawModel model, string? identityName);
    }
}
