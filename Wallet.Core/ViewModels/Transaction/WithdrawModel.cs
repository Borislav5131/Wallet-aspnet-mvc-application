using System.ComponentModel.DataAnnotations;

using static Wallet.Infrastructure.Data.DataConstants.Transaction;
using static Wallet.Infrastructure.Data.DataConstants.Wallet;

namespace Wallet.Core.ViewModels.Transaction
{
    public class WithdrawModel
    {
        [Required]
        [Range(TransactionMinValue, TransactionMaxValue)]
        public decimal Value { get; set; }

        [Range(WalletBalanceMinValue, WalletBalanceMaxValue)]
        public decimal UserBalance { get; set; }
    }
}
