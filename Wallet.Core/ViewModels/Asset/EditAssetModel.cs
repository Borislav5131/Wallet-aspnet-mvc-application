using System.ComponentModel.DataAnnotations;
using static Wallet.Infrastructure.Data.DataConstants.Asset;
using static Wallet.Infrastructure.Data.DataConstants.Category;

namespace Wallet.Core.ViewModels.Asset
{
    public class EditAssetModel
    {
        public Guid AssetId { get; set; }

        [Required]
        [MaxLength(AssetMaxNameLenght, ErrorMessage = "Name must be max 30 characters!"), MinLength(AssetMinNameLenght, ErrorMessage = "Name must be min 3 characters!")]
        public string Name { get; set; }

        [Required]
        [MaxLength(AssetMaxAbbreviationLenght, ErrorMessage = "Abbreviation must be max 30 characters!"), MinLength(AssetMinAbbreviationLenght, ErrorMessage = "Abbreviation must be min 3 characters!")]
        public string Abbreviation { get; set; }

        public Guid CategoryId { get; set; }

        [Required]
        [MaxLength(CategoryMaxNameLenght, ErrorMessage = "Name must be max 30 characters!"), MinLength(CategoryMinNameLenght, ErrorMessage = "Name must be min 3 characters!")]
        public string Category { get; set; }

        [Required]
        [Range(AssetMinValue, AssetMaxValue, ErrorMessage = "Value must be between 1 and 100000")]
        public decimal Value { get; set; }

        public string Logo { get; set; }
    }
}
