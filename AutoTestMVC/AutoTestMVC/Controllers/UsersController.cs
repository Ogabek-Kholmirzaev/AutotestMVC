using AutoTestMVC.Models;
using AutoTestMVC.Repositories;
using AutoTestMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoTestMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly UsersRepository _usersRepository;
        private readonly CookiesService _cookiesService;

        public UsersController()
        {
            _usersRepository = new UsersRepository();
            _cookiesService = new CookiesService();
        }

        public IActionResult Index()
        {
            var userPhone = _cookiesService.GetUserPhoneFromCookie(HttpContext);

            if (userPhone != null)
            {
                var user = _usersRepository.GetUserByPhone(userPhone);
                if (user.Phone == userPhone)
                {
                    return View(user);
                }
            }

            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult Signin()
        {
            return View();
        }

        public IActionResult SignupPost(User user)
        {
            _usersRepository.InsertUser(user);
            _cookiesService.SendUserPhoneToCookie(user.Phone!, HttpContext);
            return RedirectToAction("Index");
        }

        public IActionResult SigninPost(User user)
        {
            var _user = _usersRepository.GetUserByPhone(user.Phone!);

            if (user.Password == _user.Password)
            {
                _cookiesService.SendUserPhoneToCookie(user.Phone!, HttpContext);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Signin");
        }
    }
}
