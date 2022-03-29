using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels;
using Wallet.Core.ViewModels.User;
using Wallet.Infrastructure.Data.Models;

namespace Wallet.Areas.Admin.Controllers
{

    public class AdminController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IUserAssetService _userAssetService;
        private readonly ITransactionService _transactionService;
        private readonly INotyfService _notyf;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<User> userManager,
            IUserService userService,
            RoleManager<IdentityRole> roleManager, 
            INotyfService notyf,
            IUserAssetService userAssetService,
            ITransactionService transactionService)
        {
            _userManager = userManager;
            _userService = userService;
            _roleManager = roleManager;
            _notyf = notyf;
            _userAssetService = userAssetService;
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> CreateRole()
        {
            //await _roleManager.CreateAsync(new IdentityRole()
            //{
            //    Name = "Administrator"
            //});

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userService.GetUsers();

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string userId)
        {
            var model = _userService.GetDetailsOfUser(userId);

            if (model == null)
            {
                return View("Error", new ErrorViewModel() { ErrorMessage = "Something get wrong!" });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserModel model, IFormFile? image)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            byte[] convertedLogo = null;

            if (image != null)
            {
                convertedLogo = ConvertLogoToBytes(image);
            }

            var (isEdit, error) = await _userService.Edit(model, convertedLogo);

            if (!isEdit)
            {
                return View("Error", new ErrorViewModel() { ErrorMessage = error });
            }

            _notyf.Success("Successfully edit user.");
            return Redirect("/Admin/Admin/ManageUsers");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var deleted = _userService.Delete(userId);

            if (!deleted)
            {
                return View("Error", new ErrorViewModel() { ErrorMessage = "User can't be deleted!" });
            }

            _notyf.Success("Successfully delete user.");
            return Redirect("/Admin/Admin/ManageUsers");
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserAssets(string userId)
        {
            var user = _userService.GetUserById(userId);

            if (user == null)
            {
                return View("Error", new ErrorViewModel() { ErrorMessage = "Invalid user!" });
            }

            var userAssets = _userAssetService.GetUserAssetsInformation(user.UserName);

            return View(userAssets);
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserTransactions(string userId)
        {
            var user = _userService.GetUserById(userId);

            if (user == null)
            {
                return View("Error", new ErrorViewModel() { ErrorMessage = "Invalid user!" });
            }

            var userTransactions = _transactionService.GetUserTransactions(user.UserName);

            return View(userTransactions);
        }

        private byte[] ConvertLogoToBytes(IFormFile logo)
        {
            var ms = new MemoryStream();
            logo.CopyTo(ms);

            return ms.ToArray();
        }
    }
}
