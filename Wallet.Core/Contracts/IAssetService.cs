using Wallet.Core.ViewModels.Asset;
using Wallet.Infrastructure.Data.Models;

namespace Wallet.Core.Contracts
{
    public interface IAssetService
    {
        Asset? GetAssetById(Guid assetId);
        List<AllAssetViewModel> GetAssetsInCategory(Guid categoryId);
        (bool added, string error) Create(CreateAssetModel model,byte[] logo);
        bool Delete(Guid assetId);
        EditAssetModel GetDetailsOfAsset(Guid assetId);
        (bool isEdit, string error) Edit(EditAssetModel model,byte[] logo);
        BuyAssetModel GetBuyInformationOfAsset(Guid assetId, string? identityName);
        (bool isBuyed, string error) BuyAsset(BuyAssetModel model, string? identityName);
        List<AllAssetViewModel> GetAllAssets();
    }
}
