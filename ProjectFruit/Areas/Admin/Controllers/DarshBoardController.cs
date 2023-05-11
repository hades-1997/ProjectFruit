using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ProjectFruit.Areas.Admin.Services;
using ProjectFruit.Controllers;
using ProjectFruit.Models;
using ProjectFruit.Helpers;
using System.Text.Encodings.Web;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using NuGet.Packaging.Signing;
using CyberSource.Clients.SoapServiceReference;

namespace ProjectFruit.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    public class DarshBoardController : Controller
    {

        private AccountService accountService;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration configration;
        public DarshBoardController(AccountService _accountService, ILogger<HomeController> logger, IConfiguration _configration)
        {
            accountService = _accountService;
            _logger = logger;
            configration = _configration;
        }

        [Authorize(Roles = "superadmin")]
        [Route("dashboard")]
        [Route("")]
        public IActionResult Dashboard()
        {
            var username = User.FindFirst(ClaimTypes.Name);
            ViewBag.username = username.Value;
            ViewBag.users = accountService.findAll();
            return View("Dashboard");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string username, string password)
        {
            //chuyển password sang Bcrypt 
            // password = BCrypt.Net.BCrypt.HashString(password);
            //  var result = accountService.findByUsername(username, password);
            var result = accountService.PasswordSignInAsync(username, password);
            if (result != null)
            {
               if(result.Status == 0)
                {
                    ViewBag.msg = "Tài Khoản chưa được kích hoạt";
                    return View("Login");

                }
                else
                {
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(accountService.GetUserClaims(result), CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                    return RedirectToAction("dashboard", "DarshBoard", new { area = "admin" });
                }
            }
            else
            {
                ViewBag.msg = "Username và Password không tồn tại !!!";
                return View("Login");
            }
           
        }

        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public  IActionResult Register(string Username, string Email, string Password, string confirmPassword)
        {
            var userExist =  accountService.userExistsAsync(Username);
            if (userExist)
            {
                ViewBag.msg = "Username đã tồn tại !!!";
            }
            else
            {
                User user = new User();
                user.Username = Username;
                user.Md5username = MD5Helper.HashstringMd5(Username);
                user.Password = BCrypt.Net.BCrypt.HashPassword(Password);
                user.Email = Email;
                user.AuthorId = 2;
                user.Status = 0;
                accountService.Register(user);

                var mailHelper = new MailHelper(configration);

                // DateTimeOffset date_sign = DateTimeOffset.Now;
                DateTimeOffset dateTimeOffset = new DateTimeOffset(DateTime.Now);
                long timestamp = dateTimeOffset.ToUnixTimeSeconds();
                string timestampString = timestamp.ToString();
              //  DateTime date_sign = DateTime.Now;
              //  string currentTime = DataFormatHelper.FormatTime(date_sign).ToString();
              //  string codeChess = MD5Helper.HashstringMd5(currentTime);
              //  string userMD5 = MD5Helper.HashstringMd5(user.Username);
                var callbackUrl = Url.Action(
                "ConfirmEmail",
                "admin",
                new { userId = user.Md5username, code = timestampString },
                protocol: HttpContext.Request.Scheme);

                if (mailHelper.Send("saka.dacloi@gmail.com", Email, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."))
                {
                    return View("Notification");

                }

                return View("Login");
                
            }
            return View("Register");
        }

        [HttpGet]
        [Route("create-author")]
        public IActionResult CreateAuthor()
        {
            return View("CreateAuthor", new Author());
        }

        [HttpPost]
        [Route("create-author")]
        public IActionResult CreateAuthor(Author author)
        {

            return RedirectToAction("Dashboard" , "DarshBoard");
        }


        [HttpGet]
        [Route("ConfirmEmail")]
        public IActionResult ConfirmEmail(string userId, long code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Login", "DarshBoard");
            }

            //.SequenceEqual(hash2)
            //string username = MD5Helper.CheckMD5(userId).ToString();

            var username = accountService.findUserName(userId);
            //  string codeMD5 = MD5Helper.CheckMD5(code).ToString();

            DateTime today = DateTime.Now;
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(code);
            DateTime dateTimeCode = dateTimeOffset.LocalDateTime;
            TimeSpan difference = today.Subtract(dateTimeCode);

            //  string checkDate = dateNow - DateTime.Parse(codeMD5);
            if (username != null && difference.TotalDays < 1) {
               username.Status = 1;
                accountService.UpdateUser(username);
                return View("Login");
            }else
            {
                return View("confirmEmail");
            }
                    
        }

  
        [Route("notification")]
        public IActionResult Notification()
        {
            return View();
        }

    }
}
