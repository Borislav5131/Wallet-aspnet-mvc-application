using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels;
using Wallet.Core.ViewModels.Role;
using Wallet.Core.ViewModels.User;
using Wallet.Infrastructure.Data.Models;

namespace Wallet.Areas.Admin.Controllers
{

    public class AdminController : BaseController
    {
        private readonly INotyfService _notyf;
        private readonly IUserService _userService;
        private readonly IAssetService _assetService;
        private readonly UserManager<User> _userManager;
        private readonly ICategoryService _categoryService;
        private readonly IUserAssetService _userAssetService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITransactionService _transactionService;

        public AdminController(
            INotyfService notyf,
            IUserService userService,
            IAssetService assetService,
            UserManager<User> userManager,
            ICategoryService categoryService,
            IUserAssetService userAssetService,
            RoleManager<IdentityRole> roleManager,
            ITransactionService transactionService)
        {
            _notyf = notyf;
            _userService = userService;
            _assetService = assetService;
            _userManager = userManager;
            _categoryService = categoryService;
            _userAssetService = userAssetService;
            _roleManager = roleManager;
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> AllRoles()
        {
            var model = _roleManager.Roles
                .ToList()
                .Select(r=> new AllRoleViewModel()
                { 
                    Id = r.Id, 
                    Name = r.Name,
                })
                .ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateRole()
            => View();

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = model.Name
            });

            if (!result.Succeeded)
            {
                return View("Error", new ErrorViewModel() { ErrorMessage = "Something get wrong!" });
            }

            _notyf.Success("Successfully create role.");
            return RedirectToAction(nameof(AllRoles));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return View("Error", new ErrorViewModel() { ErrorMessage = "Something get wrong!" });
            }

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                return View("Error", new ErrorViewModel() { ErrorMessage = "Can't delete role!" });
            }

            _notyf.Success("Successfully delete role.");
            return RedirectToAction(nameof(AllRoles));
        }

        [HttpGet]
        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user = _userService.GetUserById(userId);

            if (user == null)
            {
                return View("Error", new ErrorViewModel() { ErrorMessage = "Something get wrong!" });
            }

            var model = new UserRolesViewModel()
            {
                UserId = user.Id,
                Username = user.UserName
            };

            ViewBag.RoleItems = _roleManager.Roles
                .ToList()
                .Select(r => new SelectListItem()
                {
                    Text = r.Name,
                    Value = r.Name,
                    Selected = _userManager.IsInRoleAsync(user, r.Name).Result
                })
                .ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageRoles(UserRolesViewModel model)
        {
            var user = _userService.GetUserById(model.UserId);
            var userRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, userRoles);

            if (model.RoleNames?.Length > 0)
            {
                await _userManager.AddToRolesAsync(user, model.RoleNames);
            }

            return RedirectToAction(nameof(ManageUsers));
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

        [HttpGet]
        public async Task<IActionResult> ManageCategories()
        {
            var categories = _categoryService.GetAllCategories();

            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> ManageAssets()
        {
            var assets = _assetService.GetAllAssets();

            return View(assets);
        }

        private byte[] ConvertLogoToBytes(IFormFile logo)
        {
            var ms = new MemoryStream();
            logo.CopyTo(ms);

            return ms.ToArray();
        }
    }
}
