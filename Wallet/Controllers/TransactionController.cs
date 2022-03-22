﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels;
using Wallet.Core.ViewModels.Transaction;

namespace Wallet.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly INotyfService _notyf;

        public TransactionController(ITransactionService transactionService,
            INotyfService notyf)
        {
            _transactionService = transactionService;
            _notyf = notyf;
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

        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}