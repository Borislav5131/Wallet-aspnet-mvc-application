using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels;
using Wallet.Core.ViewModels.Asset;

namespace Wallet.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class AssetController : Controller
    {
        private readonly IAssetService _assetService;
        private readonly ICategoryService _categoryService;
        private readonly INotyfService _notyf;

        public AssetController(IAssetService assetService, ICategoryService categoryService, INotyfService notyf)
        {
            _assetService = assetService;
            _categoryService = categoryService;
            _notyf = notyf;
        }

        [HttpGet]
        public IActionResult All(Guid categoryId)
        {
            var assets = _assetService.GetAssetsInCategory(categoryId);

            ViewData["CategoryName"] = _categoryService.GetCategoryName(categoryId);
            ViewData["CategoryId"] = categoryId;

            return View(assets);
        }

        [HttpGet]
        public IActionResult Create(Guid categoryId)
        {
            var model = _categoryService.AssetCreateFormModel(categoryId);

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateAssetModel model, IFormFile logo)
        {
            if (!ModelState.IsValid)
            {
                if (logo == null)
                {
                    ModelState.AddModelError("", "Logo is required!");
                }

                return View(model);
            }

            var convertedLogo = ConvertLogoToBytes(logo);
            var (added, error) = _assetService.Create(model, convertedLogo);

            if (!added)
            {
                ModelState.AddModelError("", error);
                return View(model);
            }

            _notyf.Success("Successfully added new asset.");
            return Redirect($"/Asset/All?categoryId={model.CategoryId}");
        }

        [HttpGet]
        public IActionResult Edit(Guid assetId)
        {
            var model = _assetService.GetDetailsOfAsset(assetId);

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditAssetModel model, IFormFile? logo)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            byte[] convertedLogo = null;

            if (logo != null)
            {
                convertedLogo = ConvertLogoToBytes(logo);
            }

            var (isEdit, error) = _assetService.Edit(model,convertedLogo);

            if (!isEdit)
            {
                return View("Error", new ErrorViewModel() { ErrorMessage = error });
            }

            _notyf.Success("Successfully edit asset.");
            return Redirect($"/Asset/All?categoryId={model.CategoryId}");
        }

        [HttpGet]
        public IActionResult Delete(Guid assetId)
        {
            var deleted = _assetService.Delete(assetId);

            if (!deleted)
            {
                return View("Error", new ErrorViewModel() { ErrorMessage = "Asset can't be deleted!" });
            }

            _notyf.Success("Successfully delete asset.");
            return Redirect("/Category/All");
        }

        private byte[] ConvertLogoToBytes(IFormFile logo)
        {
            var ms = new MemoryStream();
            logo.CopyTo(ms);

            return ms.ToArray();
        }
    }
}
