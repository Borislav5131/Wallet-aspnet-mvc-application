using Wallet.Core.ViewModels.Category;

namespace Wallet.Core.Contracts
{
    public interface ICategoryService
    {
         (bool added, string error) Add(AddFormModel model);
         List<AllViewModel> GetAllCategories();
    }
}
