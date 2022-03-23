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

        public UserViewModel GetUserInformation(string username)
            => _repo.All<User>()
                .Where(u=>u.UserName == username)
                .Select(u=> new UserViewModel()
                {
                    Username = u.UserName,
                    Email = u.Email,
                    Balance = u.Balance,
                    Image = $"data:image;base64,{Convert.ToBase64String(u.Image)}"
                })
                .First();

        public decimal GetUserBalance(string? userName)
            => _repo.All<User>()
                .Where(u => u.UserName == userName)
                .Select(u => u.Balance)
                .First();
    }
}
