using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ProjectFruit.Areas.Admin.Services;
using ProjectFruit.Controllers;
using ProjectFruit.Models;
using CyberSource.Clients.SoapServiceReference;
using ProjectFruit.Helpers;
using System.Text.Encodings.Web;

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
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(accountService.GetUserClaims(result), CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                return RedirectToAction("dashboard", "DarshBoard", new { area = "admin" });
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
                user.Password = BCrypt.Net.BCrypt.HashPassword(Password);
                user.Email = Email;
                user.AuthorId = 2;
                user.Status = 0;
                accountService.Register(user);

                var mailHelper = new MailHelper(configration);

                var callbackUrl = Url.Action(
                "ConfirmEmail",
                "admin",
                new { userId = user.Id, code = 1234567 },
                protocol: HttpContext.Request.Scheme);

                if (mailHelper.Send("saka.dacloi@gmail.com", Email, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."))
                {

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
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            //if (userId == null || code == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            //var user = await _userManager.FindByIdAsync(userId);
            //if (user == null)
            //{
            //    return NotFound($"Unable to load user with ID '{userId}'.");
            //}

            //var result = await _userManager.ConfirmEmailAsync(user, code);
            //if (!result.Succeeded)
            //{
            //    throw new InvalidOperationException($"Error confirming email for user with ID '{userId}':");
            //}

            return View();
        }

    }
}
