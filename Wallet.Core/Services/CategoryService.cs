using Wallet.Core.Constants;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels.Category;
using Wallet.Data.Models;

namespace Wallet.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository repo;

        public CategoryService(IRepository repo)
        {
            this.repo = repo;
        }

        public (bool added, string error) Add(AddCategoryFormModel model)
        {
            bool added = false;
            string error = null;

            if (repo.All<Category>().Any(c=>c.Name == model.Name))
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
                repo.Add<Category>(c);
                repo.SaveChanges();
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
            return repo.All<Category>()
                .Select(c => new AllCategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                })
                .ToList();
        }

        public bool Delete(Guid id)
        {
            var category = repo.All<Category>()
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return false;
            }

            try
            {
                repo.Remove<Category>(category);
                repo.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
