using Wallet.Core.Constants;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels.UserAsset;
using Wallet.Infrastructure.Data.Models;

namespace Wallet.Core.Services
{
    public class UserAssetService : IUserAssetService
    {
        private readonly IRepository _repo;
        private readonly ITransactionService _transactionService;
        private readonly IUserService _userService;

        public UserAssetService(IRepository repo,
            ITransactionService transactionService,
            IUserService userService)
        {
            _repo = repo;
            _transactionService = transactionService;
            _userService = userService;
        }

        public IEnumerable<UserAssetViewModel> GetUserAssetsInformation(string identityName)
            => _repo.All<Infrastructure.Data.Models.Wallet>()
                .Where(w => w.User.UserName == identityName)
                .Select(w => w.UserAssets)
                .First()
                .OrderBy(a=>a.Quantity)
                .Select(a => new UserAssetViewModel()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Abbreviation = a.Abbreviation,
                    Category = a.CategoryName,
                    BuyedPrice = a.BuyedPrice,
                    Amount = a.Amount,
                    Quantity = a.Quantity,
                    Logo = a.Logo
                });

        public (bool isSelled, string error) Sell(Guid userAssetId, string identityName)
        {
            bool isSelled = false;
            string error = String.Empty;

            var user = _userService.GetUserByName(identityName);
            var userAsset = _repo.All<UserAsset>()
                .FirstOrDefault(ua => ua.Id == userAssetId);
            var asset = _repo.All<Asset>()
                .FirstOrDefault(a => a.Name == userAsset.Name && a.Abbreviation == userAsset.Abbreviation);

            if (user == null || userAsset == null || asset == null)
            {
                return (isSelled, error = "Not found!");
            }

            user.Wallet.UserAssets.Remove(userAsset);
            user.Wallet.Balance += userAsset.Quantity * asset.Value;

            var transaction = _transactionService.CreateSellTransaction(user, userAsset.Amount, asset.Value);
            user.Wallet.Transactions.Add(transaction);

            try
            {
                _repo.Remove<UserAsset>(userAsset);
                _repo.Add<Transaction>(transaction);
                _repo.SaveChanges();
                isSelled = true;
            }
            catch (Exception)
            {
                error = "Could not save!";
            }

            return (isSelled, error);
        }
    }
}
