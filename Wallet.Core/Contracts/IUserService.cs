using Wallet.Infrastructure.Data.Models;

namespace Wallet.Core.Contracts
{
    public interface IUserService
    {
        void RegisterUserWallet(User user);
    }
}
