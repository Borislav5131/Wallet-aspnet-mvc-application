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

        public (bool added, string error) Create(CreateAssetModel model, byte[] logo)
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

        public EditAssetModel GetDetailsOfAsset(Guid assetId)
            => _repo.All<Asset>()
                .Where(a => a.Id == assetId)
                .Select(a => new EditAssetModel()
                {
                    AssetId = a.Id,
                    Name = a.Name,
                    Abbreviation = a.Abbreviation,
                    CategoryId = a.CategoryId,
                    Category = a.Category.Name,
                    Value = a.Value,
                    Logo = "data:image;base64," + Convert.ToBase64String(a.Logo)
                })
                .FirstOrDefault();

        public (bool isEdit, string error) Edit(EditAssetModel model,byte[] logo)
        {
            bool isEdit = false;
            string error = null;

            var asset = _repo.All<Asset>()
                .FirstOrDefault(a => a.Id == model.AssetId);
            var category = _repo.All<Category>()
                .FirstOrDefault(c => c.Id == model.CategoryId);

            if (asset == null || category == null)
            {
                return (isEdit, error = "Invalid operation");
            }

            asset.Name = model.Name;
            asset.Abbreviation = model.Abbreviation;
            asset.Value = model.Value;

            if (asset.Category.Name != model.Category)
            {
                var newCategory = _repo.All<Category>().FirstOrDefault(c => c.Name == model.Category);

                if (newCategory == null)
                {
                    return (isEdit, error = "Invalid operation");
                }

                asset.Category = newCategory;
                asset.CategoryId = newCategory.Id;
            }

            if (logo != null)
            {
                if (logo.Length > 2 * 1024 * 1024)
                {
                    return (isEdit, error = "Logo must be max 2 MB");
                }

                asset.Logo = logo;
            }

            try
            {
                _repo.SaveChanges();
                isEdit = true;
            }
            catch (Exception)
            {
                error = "Invalid operation!";
            }

            return (isEdit, error);
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
    }
}
