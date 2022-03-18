using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wallet.Core.Contracts;
using Wallet.Infrastructure.Data.Models;

using static Wallet.Core.Constants.UserConstants.Roles;


namespace Wallet.Controllers
{
    [Authorize]
    public class UserController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<User> userManager,
            IUserService userService,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userService = userService;
            _roleManager = roleManager;
        }

        
    }
}
