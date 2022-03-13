using Wallet.Core.ViewModels.Asset;

namespace Wallet.Core.Contracts
{
    public interface IAssetService
    {
        List<AllAssetViewModel> GetAssetsInCategory(Guid categoryId);
    }
}
