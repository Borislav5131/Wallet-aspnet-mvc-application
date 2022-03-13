using Wallet.Core.ViewModels.Category;

namespace Wallet.Core.Contracts
{
    public interface ICategoryService
    {
         (bool added, string error) Create(CreateCategoryFormModel model);
         List<AllCategoryViewModel> GetAllCategories();
         bool Delete(Guid categoryId);
         string GetCategoryName(Guid categoryId);
    }
}
