using Wallet.Core.Constants;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels.UserAsset;

namespace Wallet.Core.Services
{
    public class UserAssetService : IUserAssetService
    {
        private readonly IRepository _repo;

        public UserAssetService(IRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<UserAssetViewModel> GetUserAssetsInformation(string identityName)
            => _repo.All<Infrastructure.Data.Models.Wallet>()
                .Where(w => w.User.UserName == identityName)
                .Select(w => w.UserAssets)
                .First()
                .OrderBy(a=>a.Quantity)
                .Select(a => new UserAssetViewModel()
                {
                    Name = a.Name,
                    Abbreviation = a.Abbreviation,
                    Category = a.CategoryName,
                    BuyedPrice = a.BuyedPrice,
                    Amount = a.Amount,
                    Quantity = a.Quantity
                });
    }
}
