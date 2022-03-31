using System.Dynamic;

namespace Wallet.Core.ViewModels.Asset
{
    public class AllAssetViewModel
    {
        public Guid AssetId { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Logo { get; set; }
    }
}
