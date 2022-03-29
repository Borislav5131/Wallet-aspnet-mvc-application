namespace Wallet.Core.ViewModels.UserAsset
{
    public class UserAssetViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Category { get; set; }
        public decimal BuyedPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal Quantity { get; set; }
    }
}
