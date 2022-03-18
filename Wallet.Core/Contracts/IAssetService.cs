using Wallet.Core.ViewModels.Asset;

namespace Wallet.Core.Contracts
{
    public interface IAssetService
    {
        List<AllAssetViewModel> GetAssetsInCategory(Guid categoryId);
        (bool added, string error) Create(CreateAssetModel model,byte[] logo);
        bool Delete(Guid assetId);
        EditAssetModel GetDetailsOfAsset(Guid assetId);
        (bool isEdit, string error) Edit(EditAssetModel model,byte[] logo);
    }
}
