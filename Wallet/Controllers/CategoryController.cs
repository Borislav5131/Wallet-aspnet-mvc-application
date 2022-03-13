using Microsoft.AspNetCore.Mvc;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels;
using Wallet.Core.ViewModels.Category;

namespace Wallet.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult All()
        {
            var categories = categoryService.GetAllCategories();

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(CreateCategoryFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var (added, error) = categoryService.Create(model);

            if (!added)
            {
                ModelState.AddModelError("",error);
                return View();
            }

            return Redirect("/Category/All");
        }


        public IActionResult Edit() => View();

        public IActionResult Details() => View();

        public IActionResult Delete(Guid categoryId)
        {
            var deleted = categoryService.Delete(categoryId);

            if (!deleted)
            {
                return View("Error", new ErrorViewModel(){ErrorMessage = "Category can't be deleted!"});
            }

            return Redirect("/Category/All");
        }
    }
}
