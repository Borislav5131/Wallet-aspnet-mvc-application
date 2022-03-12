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
            var (added, error) = categoryService.Add(model);

            if (added)
            {
                return Redirect("/Category/All");
            }

            return View();
        }


        public IActionResult Edit() => View();

        public IActionResult Details() => View();

        public IActionResult Delete() => View();
    }
}
