using Wallet.Core.Contracts;
using Wallet.Core.ViewModels.Category;
using Wallet.Data.Models;
using Wallet.Infrastructure.Data;

namespace Wallet.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly WalletDbContext context;

        public CategoryService(WalletDbContext context)
        {
            this.context = context;
        }

        public (bool added, string error) Add(AddFormModel model)
        {
            bool added = false;
            string error = null;

            Category c = new Category()
            {
                Name = model.Name,
                Description = model.Description
            };

            try
            {
                context.Categories.Add(c);
                context.SaveChanges();
                added = true;
            }
            catch (Exception)
            {
                error = "Cound not save category!";
            }
            
            return (added, error);
        }

        public List<AllViewModel> GetAllCategories()
        {
            return context.Categories
                .Select(c => new AllViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                })
                .ToList();
        }
    }
}
