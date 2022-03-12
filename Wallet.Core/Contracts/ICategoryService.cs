using Wallet.Core.ViewModels.Category;

namespace Wallet.Core.Contracts
{
    public interface ICategoryService
    {
         (bool added, string error) Add(AddCategoryFormModel model);
         List<AllCategoryViewModel> GetAllCategories();
         bool Delete(Guid id);
    }
}
