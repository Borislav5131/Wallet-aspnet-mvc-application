namespace Wallet.Core.ViewModels.Transaction
{
    public class UserTransactionsViewModel
    {
        public string Username { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public decimal Value { get; set; }
        public decimal Amont { get; set; }
    }
}
