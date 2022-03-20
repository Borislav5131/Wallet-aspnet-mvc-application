using System.ComponentModel.DataAnnotations;
using static Wallet.Infrastructure.Data.DataConstants.Transaction;

namespace Wallet.Infrastructure.Data.Models
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();

        [Required]
        public User User { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TransactionTypes Type { get; set; }

        [Required]
        [Range(TransactionMinValue, TransactionMaxValue)]
        public decimal Value { get; set; }

        [Range(TransactionMinAmount, TransactionMaxAmount)]
        public decimal Amount { get; set; }
    }
}
