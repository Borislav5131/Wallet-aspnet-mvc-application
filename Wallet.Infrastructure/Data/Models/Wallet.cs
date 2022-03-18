using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Wallet.Infrastructure.Data.DataConstants.Wallet;

namespace Wallet.Infrastructure.Data.Models
{
    public class Wallet
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [Range(WalletTotalAmountMinValue,WalletTotalAmountMaxValue)]
        public decimal TotalValue { get; set; }

        public List<Asset> Assets { get; set; }
    }
}
