using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ProjectFruit.Areas.Admin.Services;
using ProjectFruit.Controllers;
using ProjectFruit.Models;

namespace ProjectFruit.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    public class DarshBoardController : Controller
    {

        private AccountService accountService;
        private readonly ILogger<HomeController> _logger;

        public DarshBoardController(AccountService _accountService, ILogger<HomeController> logger)
        {
            accountService = _accountService;
            _logger = logger;
        }

        [Authorize(Roles = "superadmin")]
        [Route("dashboard")]
        [Route("")]
        public IActionResult Dashboard()
        {
            var user = User.FindFirst(ClaimTypes.Name);
            ViewBag.username = user.Value;
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
            return View("Register", new User());
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(User users)
        {

           if(ModelState.IsValid)
            {
                users.Password = BCrypt.Net.BCrypt.HashPassword(users.Password);
                users.AuthorId = 2;
                users.Status = 0;
                accountService.Register(users);
                return View("Login");
            }

            ViewBag.msg = "Dữ liệu không được để trống !!!";
            return View("Register");
        }

    }
}
