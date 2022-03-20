using Microsoft.EntityFrameworkCore;
using Wallet.Core.Constants;
using Wallet.Core.Contracts;
using Wallet.Core.ViewModels.User;
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

        public async Task<IEnumerable<UserListViewModel>> GetUsers()
            => await _repo.All<User>()
                .Select(u => new UserListViewModel()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email
                })
                .ToListAsync();

        public User? GetUserByName(string user)
            => _repo.All<User>()
                .FirstOrDefault(u => u.UserName == user);
    }
}
