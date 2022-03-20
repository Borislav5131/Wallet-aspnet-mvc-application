using System.ComponentModel.DataAnnotations;

using static Wallet.Infrastructure.Data.DataConstants.Transaction;

namespace Wallet.Core.ViewModels.Transaction
{
    public class DepositModel
    {
        [Required]
        [Range(TransactionMinValue,TransactionMaxValue)]
        public decimal Value { get; set; }
    }
}
