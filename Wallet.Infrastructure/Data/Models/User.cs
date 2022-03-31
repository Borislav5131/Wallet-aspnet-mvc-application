using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Wallet.Infrastructure.Data.DataConstants.User;

namespace Wallet.Infrastructure.Data.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            Wallet = new Wallet()
            {
                UserId = Id,
            };
        }

        [MaxLength(UserMaxImageSize)]
        public byte[] Image { get; set; }

        [Required]
        public Wallet Wallet { get; set; }

    }
}
