using Wallet.Core.Constants;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels.Asset;
using Wallet.Infrastructure.Data.Models;

namespace Wallet.Core.Services
{
    public class AssetService : IAssetService
    {
        private readonly IRepository _repo;

        public AssetService(IRepository repo)
        {
            _repo = repo;
        }

        public (bool added, string error) Create(CreateAssetFormModel model, byte[] logo)
        {
            bool added = false;
            string error = null;

            if (_repo.All<Asset>().Any(c => c.Name == model.Name))
            {
                return (added, error = "Asset exist!");
            }

            if (logo.Length > 2 * 1024 * 1024)
            {
                return (added, error = "Logo must be max 2 MB");
            }

            var category = _repo.All<Category>().FirstOrDefault(c => c.Name == model.CategoryName);

            if (category == null)
            {
                return (added, error = "Category is not valid!");
            }

            Asset a = new Asset()
            {
                Name = model.Name,
                Abbreviation = model.Abbreviation,
                CategoryId = category.Id,
                Category = category,
                Value = model.Value,
                Logo = logo
            };

            category.Assets.Add(a);

            try
            {
                _repo.Add<Asset>(a);
                _repo.SaveChanges();
                added = true;
            }
            catch (Exception)
            {
                error = "Cound not create asset!";
            }

            return (added, error);
        }

        public bool Delete(Guid assetId)
        {
            var asset = _repo.All<Asset>()
                .FirstOrDefault(a => a.Id == assetId);
            var category = _repo.All<Category>()
                .FirstOrDefault(c => c.Id == asset.CategoryId);

            if (asset == null || category == null)
            {
                return false;
            }

            try
            {
                category.Assets.Remove(asset);
                _repo.Remove<Asset>(asset);
                _repo.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public List<AllAssetViewModel> GetAssetsInCategory(Guid categoryId)
          => _repo.All<Asset>()
                .Where(a => a.Category.Id == categoryId)
                .Select(a => new AllAssetViewModel()
                {
                    AssetId = a.Id,
                    Name = a.Name,
                    Abbreviation = a.Abbreviation,
                    Price = a.Value,
                    Logo = "data:image;base64," + Convert.ToBase64String(a.Logo)
                })
                .ToList();
    }
}
