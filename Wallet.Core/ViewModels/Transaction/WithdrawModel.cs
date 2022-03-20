using System.ComponentModel.DataAnnotations;

using static Wallet.Infrastructure.Data.DataConstants.Transaction;
using static Wallet.Infrastructure.Data.DataConstants.User;

namespace Wallet.Core.ViewModels.Transaction
{
    public class WithdrawModel
    {
        [Required]
        [Range(TransactionMinValue, TransactionMaxValue)]
        public decimal Value { get; set; }

        [Range(UserBalanceMinValue, UserBalanceMaxValue)]
        public decimal UserBalance { get; set; }
    }
}
