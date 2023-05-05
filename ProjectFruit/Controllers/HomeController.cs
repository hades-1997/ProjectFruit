using Microsoft.AspNetCore.Mvc;
using ProjectFruit.Areas.Admin.Services;
using ProjectFruit.Models;
using System.Diagnostics;

namespace ProjectFruit.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private AccountService accountService;

        public HomeController(ILogger<HomeController> logger, AccountService _accountService)
        {
            _logger = logger;
            accountService = _accountService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            return View("Add", new User());
            //turn View();
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(User user)
        {
            accountService.Register(user);
            return View("Index");
        }


        public IActionResult Privacy()
        {
           
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}