using ProjectFruit.Models;
using System.Security.Claims;

namespace ProjectFruit.Areas.Admin.Services
{
    public class AccountServiceImpl : AccountService
    {
        private DatabaseContext dbContext;

        public AccountServiceImpl(DatabaseContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public List<User> findAll()
        {
            return dbContext.Users.ToList();
        }

        public User PasswordSignInAsync(string username, string password)
        {
            var users = dbContext.Users.FirstOrDefault(u => u.Username == username);
            if (users != null)
            {
                bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, users.Password);
                if (isPasswordCorrect)
                {
                    return users;
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;

            }
            //   return dbContext.Users.FirstOrDefault(u => u.Username = username);
            // bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, pa);
        }

        public IEnumerable<Claim> GetUserClaims(User user)
        {
            List<Claim> claims = new List<Claim>();
            // Possible null reference argument.
            claims.Add(new Claim(ClaimTypes.Name, user.Username));
            // Possible null reference argument.
            claims.Add(new Claim(ClaimTypes.Role, user.Author.AuthorName));
            //foreach (var role in user.Author.AuthorName)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role));
            //}
            return claims;
        }

        public User Register(User user)
        {

            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            return user;
        }
    }
}
