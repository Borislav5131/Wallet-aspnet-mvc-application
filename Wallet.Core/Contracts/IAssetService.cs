using Wallet.Core.ViewModels.Asset;

namespace Wallet.Core.Contracts
{
    public interface IAssetService
    {
        List<AllAssetViewModel> GetAssetsInCategory(Guid categoryId);
        (bool added, string error) Create(CreateAssetFormModel model,byte[] logo);
        bool Delete(Guid assetId);
    }
}
