using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Wallet.Infrastructure.Data.DataConstants.Asset;

namespace Wallet.Infrastructure.Data.Models
{
    public class Asset
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();

        [Required]
        [MaxLength(AssetMaxNameLenght)]
        public string Name { get; set; }

        [Required]
        [MaxLength(AssetMaxAbbreviationLenght)]
        public string Abbreviation { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        [Required]
        [Range(AssetMinValue,AssetMaxValue)]
        public decimal Value { get; set; }
        
        [Range(AssetMinAmount,AssetMaxAmount)]
        public decimal Amount { get; set; }

        [Range(AssetMinQuantity,AssetMaxQuantity)]
        public decimal Quantity { get; set; }

        [Required]
        [MaxLength(AssetMaxLogoSize)]
        public byte[] Logo { get; set; }
    }
}
