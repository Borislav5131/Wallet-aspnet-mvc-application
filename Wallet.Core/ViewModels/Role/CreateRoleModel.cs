using System.ComponentModel.DataAnnotations;

using static Wallet.Infrastructure.Data.DataConstants.Role;

namespace Wallet.Core.ViewModels.Role
{
    public class CreateRoleModel
    {
        [MaxLength(RoleMaxNameLenght),MinLength(RoleMinNameLenght)]
        public string Name { get; set; }
    }
}
