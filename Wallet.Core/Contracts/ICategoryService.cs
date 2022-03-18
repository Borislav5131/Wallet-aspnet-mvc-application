using Wallet.Core.ViewModels.Asset;
using Wallet.Core.ViewModels.Category;
using Wallet.Infrastructure.Data.Models;

namespace Wallet.Core.Contracts
{
    public interface ICategoryService
    {
         (bool added, string error) Create(CreateCategoryFormModel model);
         List<AllCategoryViewModel> GetAllCategories();
         bool Delete(Guid categoryId);
         string GetCategoryName(Guid categoryId);
         CreateAssetModel AssetCreateFormModel (Guid categoryId);
    }
}
