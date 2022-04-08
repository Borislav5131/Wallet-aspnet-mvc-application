using Microsoft.AspNetCore.Mvc;
using Wallet.Core.Contracts;

namespace Wallet.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            var model = _userService.GetInformationOfEntities();

            return View(model);
        }
    }
}
