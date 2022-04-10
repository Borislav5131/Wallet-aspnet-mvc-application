using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels;
using Wallet.Core.ViewModels.Category;

using static Wallet.Core.Constants.UserConstants.Roles;

namespace Wallet.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly ICategoryService _categoryService;

        public CategoryController(
            INotyfService notyf,
            ICategoryService categoryService)
        {
            _notyf = notyf;
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult All()
        {
            var categories = _categoryService.GetAllCategories();

            return View(categories);
        }

        [HttpGet]
        [Authorize(Roles = Administrator)]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(CreateCategoryFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var (added, error) = _categoryService.Create(model);

            if (!added)
            {
                ModelState.AddModelError("",error);
                return View();
            }

            _notyf.Success("Successfully added new category.");
            return Redirect("/Category/All");
        }

        [HttpGet]
        [Authorize(Roles = Administrator)]
        public IActionResult Edit(Guid categoryId)
        {
            var model = _categoryService.GetDetailsOfCategory(categoryId);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Administrator)]
        public IActionResult Edit(EditCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (isEdit, error) = _categoryService.Edit(model);

            if (!isEdit)
            {
                return View("Error", new ErrorViewModel() { ErrorMessage = error });
            }

            _notyf.Success("Successfully edit asset.");
            return Redirect($"/Category/Edit?categoryId={model.CategoryId}");
        }

        [HttpGet]
        [Authorize(Roles = Administrator)]
        public IActionResult Delete(Guid categoryId)
        {
            var deleted = _categoryService.Delete(categoryId);

            if (!deleted)
            {
                return View("Error", new ErrorViewModel(){ErrorMessage = "Category can't be deleted!"});
            }

            _notyf.Success("Successfully delete category.");
            return Redirect("/Category/All");
        }
    }
}
