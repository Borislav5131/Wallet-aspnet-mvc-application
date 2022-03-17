﻿using System.Dynamic;
using AspNetCoreHero.ToastNotification.Abstractions;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels;
using Wallet.Core.ViewModels.Asset;

namespace Wallet.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class AssetController : Controller
    {
        private readonly IAssetService _assetService;
        private readonly ICategoryService _categoryService;
        private readonly INotyfService _notyf;

        public AssetController(IAssetService assetService, ICategoryService categoryService, INotyfService notyf)
        {
            this._assetService = assetService;
            this._categoryService = categoryService;
            _notyf = notyf;
        }

        [HttpGet]
        public IActionResult All(Guid categoryId)
        {
            var assets = _assetService.GetAssetsInCategory(categoryId);

            ViewData["CategoryName"] = _categoryService.GetCategoryName(categoryId);
            ViewData["CategoryId"] = categoryId;

            if (assets == null ||
                ViewData["CategoryName"] == null)
            {
                return View("Error", new ErrorViewModel(){ErrorMessage = "Category is invalid!" });
            }

            
            return View(assets);
        }

        [HttpGet]
        public IActionResult Create(Guid categoryId)
        {
            var model = new CreateAssetFormModel()
            {
                CategoryId = categoryId,
                CategoryName = _categoryService.GetCategoryName(categoryId)
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateAssetFormModel model, IFormFile logo)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (logo == null)
            {
                _notyf.Error("Logo is necessary");
                return View(model);
            }

            var convertedLogo = ConvertLogoToBytes(logo);
            var (added, error) = _assetService.Create(model,convertedLogo);

            if (!added)
            {
                ModelState.AddModelError("", error);
                return View(model);
            }

            _notyf.Success("Successfully added new asset.");
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