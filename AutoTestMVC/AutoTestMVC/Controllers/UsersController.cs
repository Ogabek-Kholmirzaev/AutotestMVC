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
        private readonly UsersService _usersService;
        private readonly TicketsRepository _ticketsRepository;
        private readonly QuestionsRepository _questionsRepository;

        public UsersController()
        {
            _usersRepository = new UsersRepository();
            _cookiesService = new CookiesService();
            _usersService = new UsersService();
            _ticketsRepository = new TicketsRepository();
            _questionsRepository = new QuestionsRepository();
        }

        public IActionResult Index()
        {
            var user = _usersService.GetUserFromCookie(HttpContext);
            if (user == null)
                return RedirectToAction("Signin", "Users");

            return View(user);
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
            var _user = _usersRepository.GetUserByPhone(user.Phone!);
            _ticketsRepository.InsertUserTrainingTickets(_user.Index, _questionsRepository.GetQuestionsCount() / 20, 20);
            _cookiesService.SendUserPhoneToCookie(user.Phone!, HttpContext);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Signin(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

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
