using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddFormModel model)
        {
            var (added, error) = categoryService.Add(model);

            if (added)
            {
                return Redirect("/Category/All");
            }

            return View();
        }


        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }
    }
}
