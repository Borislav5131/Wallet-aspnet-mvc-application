﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels.Home;

namespace Wallet.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Wallet.Core.ViewModels;

    public class HomeController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly ITransactionService _transactionService;

        public HomeController(INotyfService notyf,
            ITransactionService transactionService)
        {
            _notyf = notyf;
            _transactionService = transactionService;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var model = new HomeViewModel()
                {
                    UserTransactionsViewModel = _transactionService.GetUserTransactions(User.Identity.Name)
                };

                return View("Home",model);
            }
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