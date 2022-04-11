using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels;
using Wallet.Core.ViewModels.Transaction;

using static Wallet.Core.Constants.UserConstants.Roles;

namespace Wallet.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly ITransactionService _transactionService;

        public TransactionController(
            INotyfService notyf,
            ITransactionService transactionService)
        {
            _notyf = notyf;
            _transactionService = transactionService;
        }

        [HttpGet]
        public IActionResult Deposit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Deposit(DepositModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var (isDeposit, error) = _transactionService.Deposit(model, User.Identity.Name);

            if (!isDeposit)
            {
                return View("Error", new ErrorViewModel() { ErrorMessage = error });
            }

            _notyf.Success("Successfully deposit!");
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult Withdraw(string user)
        {
            var model = _transactionService.GetUserWithdrawModel(user);

            if (model == null)
            {
                return View("Error", new ErrorViewModel() { ErrorMessage = "Invaid operation!"});
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Withdraw(WithdrawModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (isWithdraw, error) = _transactionService.Withdraw(model, User.Identity.Name);

            if (!isWithdraw)
            {
                return View("Error", new ErrorViewModel() { ErrorMessage = error });
            }

            _notyf.Success("Successfully withdraw!");
            return Redirect("/");
        }


    }
}
