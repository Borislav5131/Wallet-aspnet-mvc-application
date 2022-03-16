using AspNetCoreHero.ToastNotification.Abstractions;

namespace Wallet.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using Wallet.Core.ViewModels;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INotyfService _notyf;

        public HomeController(ILogger<HomeController> logger, INotyfService notyf)
        {
            _logger = logger;
            _notyf = notyf;
        }

        public IActionResult Index()
        {
            _notyf.Success("Welcome");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { ErrorMessage = "Something wrong!"});
        }
    }
}