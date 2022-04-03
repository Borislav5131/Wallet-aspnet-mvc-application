using Microsoft.Extensions.Caching.Memory;
using Wallet.Core.Constants;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels.Asset;
using Wallet.Infrastructure.Data.Models;

using static Wallet.Core.Constants.CacheConstants;

namespace Wallet.Core.Services
{
    public class AssetService : IAssetService
    {
        private readonly IRepository _repo;
        private readonly IMemoryCache _cache;
        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;
        private readonly ITransactionService _transactionService;

        public AssetService(
            IRepository repo,
            IMemoryCache cache,
            IUserService userService,
            ICategoryService categoryService,
            ITransactionService transactionService)
        {
            _repo = repo;
            _cache = cache;
            _userService = userService;
            _categoryService = categoryService;
            _transactionService = transactionService;
        }

        public List<AllAssetViewModel> GetAllAssets()
            => _repo.All<Asset>()
                .Select(a => new AllAssetViewModel()
                {
                    AssetId = a.Id,
                    Name = a.Name,
                    Abbreviation = a.Abbreviation,
                    Category = a.Category.Name,
                    Price = a.Value,
                    Logo = "data:image;base64," + Convert.ToBase64String(a.Logo)
                })
                .OrderBy(a=>a.Name)
                .ToList();

        public Asset? GetAssetById(Guid assetId)
            => _repo.All<Asset>()
                .FirstOrDefault(a => a.Id == assetId);

        public List<AllAssetViewModel> GetAssetsInCategory(Guid categoryId)
        {
            var allAssets = _cache.Get<List<AllAssetViewModel>>( $"{AllAssetsInCategoryCacheKey}{categoryId}");

            if (allAssets == null)
            {
                allAssets = _repo.All<Asset>()
                    .Where(a => a.Category.Id == categoryId)
                    .Select(a => new AllAssetViewModel()
                    {
                        AssetId = a.Id,
                        Name = a.Name,
                        Abbreviation = a.Abbreviation,
                        Price = a.Value,
                        Logo = "data:image;base64," + Convert.ToBase64String(a.Logo)
                    })
                    .OrderBy(a => a.Name)
                    .ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                _cache.Set($"{AllAssetsInCategoryCacheKey}{categoryId}", allAssets, cacheOptions);
            }

            return allAssets;
        }

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

            var category = _categoryService.GetCategoryById(model.CategoryId);

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

        public (bool isEdit, string error) Edit(EditAssetModel model, byte[] logo)
        {
            bool isEdit = false;
            string error = null;

            var asset = GetAssetById(model.AssetId);
            var category = _categoryService.GetCategoryById(model.CategoryId);

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

        public (bool isBuyed, string error) BuyAsset(BuyAssetModel model, string username)
        {
            bool isBuyed = false;
            string error = String.Empty;

            var asset = GetAssetById(model.AssetId);
            var user = _userService.GetUserByName(username);

            if (asset == null || user == null)
            {
                return (isBuyed, error = "Not found!");
            }

            if (model.Amount > model.UserBalance)
            {
                return (isBuyed, error = "Not enough money!");
            }

            var userAsset = new UserAsset()
            {
                Name = model.Name,
                Abbreviation = model.Abbreviation,
                CategoryName = model.CategoryName,
                BuyedPrice = model.Value,
                Amount = model.Amount,
                Quantity = model.Quantity,
                Logo = model.Logo
            };

            user.Wallet.UserAssets.Add(userAsset);
            user.Wallet.Balance -= userAsset.Amount;

            var transaction = _transactionService.CreateBuyTransaction(user, model.Amount, model.Value);
            user.Wallet.Transactions.Add(transaction);

            try
            {
                _repo.Add<UserAsset>(userAsset);
                _repo.Add<Transaction>(transaction);
                _repo.SaveChanges();
                isBuyed = true;
            }
            catch (Exception)
            {
                error = "Could not save!";
            }

            return (isBuyed, error);
        }

        public bool Delete(Guid assetId)
        {
            var asset = GetAssetById(assetId);

            if (asset == null)
            {
                return false;
            }

            var category = _categoryService.GetCategoryById(asset.CategoryId);

            if (category == null)
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
                .First();

        public BuyAssetModel GetBuyInformationOfAsset(Guid assetId, string? userName)
            => _repo.All<Asset>()
                .Where(a => a.Id == assetId)
                .Select(a => new BuyAssetModel()
                {
                    AssetId = a.Id,
                    Name = a.Name,
                    Abbreviation = a.Abbreviation,
                    CategoryName = a.Category.Name,
                    Value = a.Value,
                    Logo = "data:image;base64," + Convert.ToBase64String(a.Logo),
                    UserBalance = _userService.GetUserBalance(userName)
                })
                .First();

    }
}
