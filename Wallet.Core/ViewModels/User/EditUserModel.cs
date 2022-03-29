using System.ComponentModel.DataAnnotations;
using static Wallet.Infrastructure.Data.DataConstants.User;

namespace Wallet.Core.ViewModels.User
{
    public class EditUserModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(UserNameMaxLenght, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = UserNameMinLenght)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? PhoneNumber { get; set; }


        public string Image { get; set; }
    }
}
