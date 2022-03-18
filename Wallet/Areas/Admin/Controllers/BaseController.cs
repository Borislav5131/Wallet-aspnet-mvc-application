using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Wallet.Core.Constants.UserConstants.Roles;

namespace Wallet.Areas.Admin.Controllers
{

    [Authorize(Roles = Administrator)]
    [Area("Admin")]
    public class BaseController : Controller
    {
        
    }
}
