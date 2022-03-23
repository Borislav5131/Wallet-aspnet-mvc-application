using System.ComponentModel.DataAnnotations;

using static Wallet.Infrastructure.Data.DataConstants.UserAsset;

namespace Wallet.Infrastructure.Data.Models
{
    public class UserAsset
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();

        [Required]
        [MaxLength(UserAssetMaxNameLenght)]
        public string Name { get; set; }

        [Required]
        [MaxLength(UserAssetMaxAbbreviationLenght)]
        public string Abbreviation { get; set; }

        [Required]
        [MaxLength(UserAssetCategoryMaxNameLenght)]
        public string CategoryName { get; set; }

        [Required]
        [Range(UserAssetMinValue, UserAssetMaxValue)]
        public decimal BuyedPrice { get; set; }

        [Range(UserAssetMinAmount, UserAssetMaxAmount)]
        public decimal Amount { get; set; }

        [Range(UserAssetMinQuantity, UserAssetMaxQuantity)]
        public decimal Quantity { get; set; }
    }
}
