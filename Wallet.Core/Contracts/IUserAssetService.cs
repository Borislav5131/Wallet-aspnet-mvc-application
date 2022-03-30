using Wallet.Core.ViewModels.UserAsset;

namespace Wallet.Core.Contracts
{
    public interface IUserAssetService
    {
        IEnumerable<UserAssetViewModel> GetUserAssetsInformation(string identityName);
        (bool isSelled, string error) Sell(Guid userAssetId, string identityName);
    }
}
