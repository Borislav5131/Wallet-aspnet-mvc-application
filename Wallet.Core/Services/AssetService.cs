using Wallet.Core.Constants;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels.Asset;
using Wallet.Data.Models;

namespace Wallet.Core.Services
{
    public class AssetService : IAssetService
    {
        private readonly IRepository repo;

        public AssetService(IRepository repo)
        {
            this.repo = repo;
        }

        public List<AllAssetViewModel> GetAssetsInCategory(Guid categoryId)
        {
            var category = repo.All<Category>()
                .FirstOrDefault(c => c.Id == categoryId);

            if (category == null)
            {
                return null;
            }

            return category.Assets
                .Select(a => new AllAssetViewModel()
                {
                    AssetId = a.Id,
                    Name = a.Name,
                    Abbreviation = a.Abbreviation,
                    Price = a.Value,
                })
                .ToList();
        }
    }
}
