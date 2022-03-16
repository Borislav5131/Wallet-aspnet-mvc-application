using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using static Wallet.Infrastructure.Data.DataConstants;

namespace Wallet.Infrastructure.Data.Models
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(UserFullNameMaxLenght)]
        public string FullName { get; set;}
    }
}
