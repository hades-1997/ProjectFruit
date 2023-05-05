using ProjectFruit.Models;
using System.Security.Claims;

namespace ProjectFruit.Areas.Admin.Services
{
    public interface AccountService
    {
        public List<User> findAll();
        public User PasswordSignInAsync(string username, string password);

        public bool userExistsAsync(string username);
        public IEnumerable<Claim> GetUserClaims(User user);

        public User Register(User user);

    }
}
