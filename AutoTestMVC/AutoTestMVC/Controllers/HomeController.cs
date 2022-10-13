using AutoTestMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AutoTestMVC.Services;

namespace AutoTestMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly UsersService _usersService;

        public HomeController(UsersService usersService)
        {
            _usersService = usersService;
        }

        public IActionResult Index()
        {
            var user = _usersService.GetUserFromCookie(HttpContext);
            ViewBag.isLogin = true;

            if (user == null)
                ViewBag.isLogin = false;

            return View();
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