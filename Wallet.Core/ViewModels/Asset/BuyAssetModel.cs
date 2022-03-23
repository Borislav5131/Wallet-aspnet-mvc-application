using System.ComponentModel.DataAnnotations;
using static Wallet.Infrastructure.Data.DataConstants.Asset;

namespace Wallet.Core.ViewModels.Asset
{
    public class BuyAssetModel
    {
        public Guid AssetId { get; set; }

        public decimal UserBalance { get; set; }

        public string Name { get; set; }
        public string CategoryName { get; set; }

        public string Abbreviation { get; set; }

        public decimal Value { get; set; }

        [Required]
        [Range(AssetMinAmount, AssetMaxAmount)]
        public decimal Amount { get; set; }

        [Required]
        [Range(AssetMinQuantity, AssetMaxQuantity)]
        public decimal Quantity { get; set; }

        public string Logo { get; set; }
    }
}
