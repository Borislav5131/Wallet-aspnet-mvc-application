using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels;

using static Wallet.Core.Constants.UserConstants.Roles;

namespace Wallet.Controllers
{
    [Authorize]
    public class UserAssetController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly IUserAssetService _userAssetService;

        public UserAssetController(
            INotyfService notyf,
            IUserAssetService userAssetService)
        {
            _notyf = notyf;
            _userAssetService = userAssetService;
        }

        [HttpGet]
        public IActionResult All()
        {
            var userAssets = _userAssetService.GetUserAssetsInformation(User.Identity.Name);

            return View(userAssets);
        }

        [HttpGet]
        public IActionResult Sell(Guid userAssetId)
        {
            var (isSelled, error) = _userAssetService.Sell(userAssetId,User.Identity.Name);

            if (!isSelled)
            {
                return View("Error", new ErrorViewModel() { ErrorMessage = error });
            }

            _notyf.Success("Successfully sell!");
            return Redirect("/");
        }
    }
}
