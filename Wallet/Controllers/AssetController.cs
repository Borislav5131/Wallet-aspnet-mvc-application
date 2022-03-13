using Wallet.Core.Contracts;
using Wallet.Core.ViewModels;

namespace Wallet.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class AssetController : Controller
    {
        private readonly IAssetService assetService;
        private readonly ICategoryService categoryService;

        public AssetController(IAssetService assetService, ICategoryService categoryService)
        {
            this.assetService = assetService;
            this.categoryService = categoryService;
        }

        public IActionResult All(Guid categoryId)
        {
            var assets = assetService.GetAssetsInCategory(categoryId);

            ViewData["CategoryName"] = categoryService.GetCategoryName(categoryId);

            if (assets == null ||
                ViewData["CategoryName"] == null)
            {
                return View("Error", new ErrorViewModel(){ErrorMessage = "Category is invalid!" });
            }

            
            return View(assets);
        }
    }
}
