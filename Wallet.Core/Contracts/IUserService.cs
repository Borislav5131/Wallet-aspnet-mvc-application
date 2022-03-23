using Wallet.Core.ViewModels.User;
using Wallet.Infrastructure.Data.Models;

namespace Wallet.Core.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserListViewModel>> GetUsers();
        User? GetUserByName(string user);
        UserViewModel GetUserInformation(string username);
        decimal GetUserBalance(string? userName);
    }
}
