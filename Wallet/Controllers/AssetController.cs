using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels;
using Wallet.Core.ViewModels.Asset;

using static Wallet.Core.Constants.UserConstants.Roles;

namespace Wallet.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class AssetController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly IAssetService _assetService;
        private readonly ICategoryService _categoryService;

        public AssetController(
            INotyfService notyf,
            IAssetService assetService,
            ICategoryService categoryService)
        {
            _notyf = notyf;
            _assetService = assetService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult All(Guid categoryId)
        {
            var assets = _assetService.GetAssetsInCategory(categoryId);

            if (assets == null)
            {
                return View("Error", new ErrorViewModel() { ErrorMessage = "Something get wrong!" });
            }

            ViewData["CategoryName"] = _categoryService.GetCategoryName(categoryId);
            ViewData["CategoryId"] = categoryId;

            return View(assets);
        }

        [HttpGet]
        [Authorize(Roles = Administrator)]
        public IActionResult Create(Guid categoryId)
        {
            var model = _categoryService.AssetCreateFormModel(categoryId);

            if (model == null)
            {
                return View("Error", new ErrorViewModel() { ErrorMessage = "Something get wrong!" });
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Administrator)]
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
        [Authorize(Roles = Administrator)]
        public IActionResult Edit(Guid assetId)
        {
            var model = _assetService.GetDetailsOfAsset(assetId);

            if (model == null)
            {
                return View("Error", new ErrorViewModel() { ErrorMessage = "Something get wrong!" });
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Administrator)]
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
            return Redirect($"/Asset/Edit?assetId={model.AssetId}");
        }

        [HttpGet]
        [Authorize(Roles = Administrator)]
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

        [HttpGet]
        public IActionResult Buy(Guid assetId)
        {
            var model = _assetService.GetBuyInformationOfAsset(assetId,User.Identity.Name);

            if (model == null)
            {
                return View("Error", new ErrorViewModel() { ErrorMessage = "Something get wrong!" });
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Buy(BuyAssetModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (isBuyed, error) = _assetService.BuyAsset(model, User.Identity.Name);

            if (!isBuyed)
            {
                ModelState.AddModelError(String.Empty, error);
                return View(model);
            }

            _notyf.Success($"Successfully buy {model.Abbreviation}");
            return Redirect("/");
        }

        private byte[] ConvertLogoToBytes(IFormFile logo)
        {
            var ms = new MemoryStream();
            logo.CopyTo(ms);

            return ms.ToArray();
        }
    }
}
