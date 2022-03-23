using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet.Infrastructure.Data.Models
{
    public class Wallet
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string UserId { get; set; }

        [Required]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public List<UserAsset> UserAssets { get; set; } = new List<UserAsset>();
    }
}
