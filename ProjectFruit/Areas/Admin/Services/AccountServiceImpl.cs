using ProjectFruit.Helpers;
using ProjectFruit.Models;
using System.Security.Claims;

namespace ProjectFruit.Areas.Admin.Services
{
    public class AccountServiceImpl : AccountService
    {
        private DatabaseContext dbContext;
        private IConfiguration config;

        public AccountServiceImpl(DatabaseContext _dbContext, IConfiguration _config)
        {
            dbContext = _dbContext;
            config = _config;
        }

        public List<User> findAll()
        {
            return dbContext.Users.ToList();
        }

        public User PasswordSignInAsync(string username, string password)
        {
           // string Hashusername = MD5Helper.HashstringMd5(username);
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

        public Boolean userExistsAsync(string username)
        {
            var result = dbContext.Users.FirstOrDefault(res => res.Username == username);
         
            if(result != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public User UpdateUser(User user)
        {
            dbContext.Users.Update(user);
            dbContext.SaveChanges();
            return user;
        }

        public User findUserName(string md5username)
        {
           var result =  dbContext.Users.FirstOrDefault(res => res.Md5username == md5username);
            return result;
        }
    }
}
