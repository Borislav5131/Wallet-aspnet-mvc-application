using Wallet.Core.ViewModels.Transaction;

namespace Wallet.Core.Contracts
{
    public interface ITransactionService
    {
        (bool isDeposit, string error) Deposit(DepositModel model, string? identityName);
    }
}
