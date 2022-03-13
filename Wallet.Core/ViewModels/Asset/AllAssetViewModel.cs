namespace Wallet.Core.ViewModels.Asset
{
    public class AllAssetViewModel
    {
        public Guid AssetId { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public decimal Price { get; set; }
    }
}
