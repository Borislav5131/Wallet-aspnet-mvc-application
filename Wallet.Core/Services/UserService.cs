using Wallet.Core.Constants;
using Wallet.Core.Contracts;
using Wallet.Infrastructure.Data.Models;

namespace Wallet.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository _repo;

        public UserService(IRepository repo)
        {
            _repo = repo;
        }

        public void RegisterUserWallet(User user)
        {
            var wallet = new Infrastructure.Data.Models.Wallet()
            {
                User = user,
                UserId = user.Id
            };

            user.Wallet = wallet;

            _repo.Add<Infrastructure.Data.Models.Wallet>(wallet);
            _repo.SaveChanges();
        }
    }
}
