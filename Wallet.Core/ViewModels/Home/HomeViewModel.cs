using Wallet.Core.ViewModels.Transaction;

namespace Wallet.Core.ViewModels.Home
{
    public class HomeViewModel
    {
        public IEnumerable<UserTransactionsViewModel> UserTransactionsViewModel { get; set; }
    }
}
