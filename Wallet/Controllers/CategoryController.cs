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
        private readonly ICategoryService _categoryService;
        private readonly INotyfService _notyf;

        public CategoryController(ICategoryService categoryService, INotyfService notyf)
        {
            _categoryService = categoryService;
            _notyf = notyf;
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

        [Authorize(Roles = Administrator)]
        public IActionResult Edit() => View();

        public IActionResult Details() => View();

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
