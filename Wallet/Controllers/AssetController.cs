using Wallet.Core.Contracts;
using Wallet.Core.ViewModels;
using Wallet.Core.ViewModels.Asset;

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

        [HttpGet]
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

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(CreateAssetFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var (added, error) = assetService.Create(model);

            if (!added)
            {
                ModelState.AddModelError("", error);
                return View();
            }

            return Redirect("/Category/All");
        }

        [HttpGet]
        public IActionResult Delete(Guid assetId)
        {
            var deleted = assetService.Delete(assetId);

            if (!deleted)
            {
                return View("Error", new ErrorViewModel() { ErrorMessage = "Asset can't be deleted!" });
            }

            return Redirect("/Category/All");
        }
    }
}
