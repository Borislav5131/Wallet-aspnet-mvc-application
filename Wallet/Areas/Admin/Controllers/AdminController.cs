using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wallet.Core.Contracts;
using Wallet.Infrastructure.Data.Models;

namespace Wallet.Areas.Admin.Controllers
{

    public class AdminController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<User> userManager,
            IUserService userService,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userService = userService;
            _roleManager = roleManager;
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
    }
}
