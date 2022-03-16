using Wallet.Core.Constants;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels.Category;
using Wallet.Infrastructure.Data.Models;

namespace Wallet.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository _repo;

        public CategoryService(IRepository repo)
        {
            this._repo = repo;
        }

        public (bool added, string error) Create(CreateCategoryFormModel model)
        {
            bool added = false;
            string error = null;

            if (_repo.All<Category>().Any(c=>c.Name == model.Name))
            {
                return (added,error = "Category exist!");
            }

            Category c = new Category()
            {
                Name = model.Name,
                Description = model.Description
            };

            try
            {
                _repo.Add<Category>(c);
                _repo.SaveChanges();
                added = true;
            }
            catch (Exception)
            {
                error = "Cound not add category!";
            }
            
            return (added, error);
        }

        public List<AllCategoryViewModel> GetAllCategories()
        {
            return _repo.All<Category>()
                .Select(c => new AllCategoryViewModel()
                {
                    CategoryId = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                })
                .ToList();
        }

        public bool Delete(Guid categoryId)
        {
            var category = _repo.All<Category>()
                .FirstOrDefault(c => c.Id == categoryId);

            if (category == null)
            {
                return false;
            }

            try
            {
                category.Assets.Clear();
                _repo.Remove<Category>(category);
                _repo.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public string GetCategoryName(Guid categoryId)
        {
            var category = _repo.All<Category>()
                .FirstOrDefault(c => c.Id == categoryId);

            if (category == null)
            {
                return null;
            }

            return category.Name;
        }
    }
}
