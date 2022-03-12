using Microsoft.AspNetCore.Mvc;
using Wallet.Core.Contracts;
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
        public IActionResult Add() => View();

        [HttpPost]
        public IActionResult Add(AddCategoryFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var (added, error) = categoryService.Add(model);

            if (!added)
            {
                ModelState.AddModelError("",error);
                return View();
            }

            return Redirect("/Category/All");
        }


        public IActionResult Edit() => View();

        public IActionResult Details() => View();

        public IActionResult Delete(Guid id)
        {
            var deleted = categoryService.Delete(id);

            if (!deleted)
            {
                ModelState.AddModelError("","Can't delete category!");
                return Redirect("/Category/All");
            }

            return Redirect("/Category/All");
        }
    }
}
