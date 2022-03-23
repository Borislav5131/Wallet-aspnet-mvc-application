using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Wallet.Infrastructure.Data.DataConstants.User;

namespace Wallet.Infrastructure.Data.Models
{
    public class User : IdentityUser
    {
        [Range(UserBalanceMinValue, UserBalanceMaxValue)]
        public decimal Balance { get; set; }

        [MaxLength(UserMaxImageSize)]
        public byte[] Image { get; set; }

        [Required]
        public Guid? WalletId { get; set; }

        [Required]
        [ForeignKey(nameof(WalletId))]
        public Wallet Wallet { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
