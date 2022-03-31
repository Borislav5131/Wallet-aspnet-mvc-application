using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels;
using Wallet.Core.ViewModels.Home;

namespace Wallet.Controllers
{
    public class HomeController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly ITransactionService _transactionService;
        private readonly IUserService _userService;
        private readonly IUserAssetService _userAssetService;

        public HomeController(INotyfService notyf,
            ITransactionService transactionService,
            IUserService userService,
            IUserAssetService userAssetService)
        {
            _notyf = notyf;
            _transactionService = transactionService;
            _userService = userService;
            _userAssetService = userAssetService;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var model = new HomeViewModel()
                {
                    UserTransactionsViewModel = _transactionService.GetUserTransactions(User.Identity.Name),
                    UserViewModel = _userService.GetUserInformation(User.Identity.Name),
                    UserAssetsViewModel = _userAssetService.GetUserAssetsInformation(User.Identity.Name)
                };

                return View("Home",model);
            }
            _notyf.Success("Welcome");
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { ErrorMessage = "Something wrong!"});
        }
    }
}