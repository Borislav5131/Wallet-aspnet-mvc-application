using Wallet.Core.Constants;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels.Asset;
using Wallet.Infrastructure.Data.Models;

namespace Wallet.Core.Services
{
    public class AssetService : IAssetService
    {
        private readonly IRepository _repo;
        private readonly IUserService _userService;
        private readonly ITransactionService _transactionService;

        public AssetService(IRepository repo,
            IUserService userService,
            ITransactionService transactionService)
        {
            _repo = repo;
            _userService = userService;
            _transactionService = transactionService;
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
                .First();

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

        public BuyAssetModel GetBuyInformationOfAsset(Guid assetId, string? userName)
            => _repo.All<Asset>()
                .Where(a=>a.Id == assetId)
                .Select(a=> new BuyAssetModel()
                {
                    AssetId = a.Id,
                    Name = a.Name,
                    Abbreviation = a.Abbreviation,
                    Value = a.Value,
                    Logo = "data:image;base64," + Convert.ToBase64String(a.Logo),
                    UserBalance = _userService.GetUserBalance(userName)
                })
                .First();

        public (bool isBuyed, string error) BuyAsset(BuyAssetModel model, string? username)
        {
            bool isBuyed = false;
            string error = String.Empty;
            
            var asset = _repo.All<Asset>().FirstOrDefault(a => a.Id == model.AssetId);
            var user = _repo.All<User>().FirstOrDefault(a => a.UserName == username);

            if (asset == null || user == null)
            {
                return (isBuyed, error = "Not found!");
            }

            if (model.Amount > model.UserBalance)
            {
                return (isBuyed, error = "Not enough money!");
            }

            var userAssets = _repo.All<Infrastructure.Data.Models.Wallet>()
                .Where(w => w.User == user)
                .Select(w => w.Assets)
                .First();

            if (!userAssets.Contains(asset))
            {
                userAssets.Add(asset);
            }

            user.Balance -= model.Amount;
            asset.Amount += model.Amount;
            asset.Quantity += model.Quantity;

            var transaction = _transactionService.CreateBuyTransaction(user,model.Amount,model.Value);
            user.Transactions.Add(transaction);

            try
            {
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
