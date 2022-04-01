using Wallet.Core.ViewModels.Asset;
using Wallet.Core.ViewModels.Category;
using Wallet.Infrastructure.Data.Models;

namespace Wallet.Core.Contracts
{
    public interface ICategoryService
    {
        Category? GetCategoryById(Guid categoryId);
        (bool added, string error) Create(CreateCategoryFormModel model);
        (bool isEdit, string error) Edit(EditCategoryModel model);
        bool Delete(Guid categoryId);
        string GetCategoryName(Guid categoryId);
        CreateAssetModel AssetCreateFormModel(Guid categoryId);
        List<AllCategoryViewModel> GetAllCategories();
        EditCategoryModel GetDetailsOfCategory(Guid categoryId);
    }
}
