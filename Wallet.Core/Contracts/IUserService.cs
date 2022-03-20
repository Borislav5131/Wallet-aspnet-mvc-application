using Wallet.Core.ViewModels.User;
using Wallet.Infrastructure.Data.Models;

namespace Wallet.Core.Contracts
{
    public interface IUserService
    {
        void RegisterUserWallet(User user);
        Task<IEnumerable<UserListViewModel>> GetUsers();
        User? GetUserByName(string user);
    }
}
