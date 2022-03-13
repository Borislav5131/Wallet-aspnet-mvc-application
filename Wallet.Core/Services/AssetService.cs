using Wallet.Core.Constants;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels.Asset;
using Wallet.Infrastructure.Data.Models;

namespace Wallet.Core.Services
{
    public class AssetService : IAssetService
    {
        private readonly IRepository repo;
        private readonly ICategoryService categoryService;

        public AssetService(IRepository repo, ICategoryService categoryService)
        {
            this.repo = repo;
            this.categoryService = categoryService;
        }

        public (bool added, string error) Create(CreateAssetFormModel model)
        {
            bool added = false;
            string error = null;

            if (repo.All<Asset>().Any(c => c.Name == model.Name))
            {
                return (added, error = "Asset exist!");
            }

            var category = categoryService.GetCategory(model.CategoryName);

            Asset a = new Asset()
            {
                Name = model.Name,
                Abbreviation = model.Abbreviation,
                Category = category,
                Value = model.Value,
            };

            category.Assets.Add(a);

            try
            {
                repo.Add<Asset>(a);
                repo.SaveChanges();
                added = true;
            }
            catch (Exception)
            {
                error = "Cound not add asset!";
            }

            return (added, error);
        }

        public List<AllAssetViewModel> GetAssetsInCategory(Guid categoryId)
        {
            return repo.All<Asset>()
                .Where(a=>a.Category.Id == categoryId)
                .Select(a => new AllAssetViewModel()
                {
                    AssetId = a.Id,
                    Name = a.Name,
                    Abbreviation = a.Abbreviation,
                    Price = a.Value,
                })
                .ToList(); ;
        }
    }
}
