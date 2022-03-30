using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;

        public UserService(IRepository repo,
            UserManager<User> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserListViewModel>> GetUsers()
            => await _repo.All<User>()
                .Select(u => new UserListViewModel()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Balance = u.Balance.ToString("F2"),
                    PhoneNumber = u.PhoneNumber,
                })
                .ToListAsync();

        public User? GetUserByName(string user)
            => _repo.All<User>()
                .FirstOrDefault(u => u.UserName == user);

        public User? GetUserById(string userId)
            => _repo.All<User>()
                .FirstOrDefault(u => u.Id == userId);

        public EditUserModel? GetDetailsOfUser(string userId)
            => _repo.All<User>()
                .Where(u => u.Id == userId)
                .Select(u => new EditUserModel()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    PhoneNumber = u.PhoneNumber,
                    Email = u.Email,
                    Image = "data:image;base64," + Convert.ToBase64String(u.Image)
                })
                .FirstOrDefault();

        public UserViewModel? GetUserInformation(string username)
            => _repo.All<User>()
                .Where(u => u.UserName == username)
                .Select(u => new UserViewModel()
                {
                    Username = u.UserName,
                    Email = u.Email,
                    Balance = u.Balance,
                    Image = $"data:image;base64,{Convert.ToBase64String(u.Image)}"
                })
                .FirstOrDefault();

        public decimal GetUserBalance(string? userName)
            => _repo.All<User>()
                .Where(u => u.UserName == userName)
                .Select(u => u.Balance)
                .First();

        public bool Delete(string userId)
        {
            var user = _repo.All<User>()
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            try
            {
                _repo.Remove<User>(user);
                _repo.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<(bool isEdit, string error)> Edit(EditUserModel model, byte[]? image)
        {
            bool isEdit = false;
            string error = null;

            var user = GetUserById(model.Id);

            if (user == null)
            {
                return (isEdit, error = "Invalid operation");
            }

            if (user.UserName != model.UserName)
            {
                var userNameExists = await _userManager.FindByNameAsync(model.UserName);

                if (userNameExists != null)
                {
                    return (isEdit, error = "Username exists!");
                }

                var setUserName = await _userManager.SetUserNameAsync(user, model.UserName);

                if (!setUserName.Succeeded)
                {
                    return (isEdit, error = "Invalid operation");
                }
            }

            if (user.Email != model.Email)
            {
                var emailExists = await _userManager.FindByEmailAsync(model.Email);

                if (emailExists != null)
                {
                    return (isEdit, error = "Email exists!");
                }

                var setEmail = await _userManager.SetEmailAsync(user, model.Email);

                if (!setEmail.Succeeded)
                {
                    return (isEdit, error = "Invalid operation");
                }
            }

            if (user.PhoneNumber != model.PhoneNumber)
            {
                var setPhoneNumber = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);

                if (!setPhoneNumber.Succeeded)
                {
                    return (isEdit, error = "Invalid operation");
                }
            }

            if (image != null)
            {
                if (image.Length > 2 * 1024 * 1024)
                {
                    return (isEdit, error = "Image must be max 2 MB");
                }

                user.Image = image;
            }

            try
            {
                await _userManager.UpdateAsync(user);
                _repo.SaveChanges();
                isEdit = true;
            }
            catch (Exception)
            {
                error = "Invalid operation!";
            }

            return (isEdit, error);
        }
    }
}
