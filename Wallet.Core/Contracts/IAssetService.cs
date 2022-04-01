using Wallet.Core.ViewModels.Asset;
using Wallet.Infrastructure.Data.Models;

namespace Wallet.Core.Contracts
{
    public interface IAssetService
    {
        List<AllAssetViewModel> GetAllAssets();
        Asset? GetAssetById(Guid assetId);
        List<AllAssetViewModel> GetAssetsInCategory(Guid categoryId);
        (bool added, string error) Create(CreateAssetModel model,byte[] logo);
        (bool isEdit, string error) Edit(EditAssetModel model, byte[] logo);
        (bool isBuyed, string error) BuyAsset(BuyAssetModel model, string? identityName);
        bool Delete(Guid assetId);
        EditAssetModel GetDetailsOfAsset(Guid assetId);
        BuyAssetModel GetBuyInformationOfAsset(Guid assetId, string? identityName);
    }
}
