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

        public UsersController(UsersRepository usersRepository, CookiesService cookiesService, UsersService usersService, TicketsRepository ticketsRepository, QuestionsRepository questionsRepository)
        {
            _usersRepository = usersRepository;
            _cookiesService = cookiesService;
            _usersService = usersService;
            _ticketsRepository = ticketsRepository;
            _questionsRepository = questionsRepository;
        }

        public IActionResult Index()
        {
            var user = _usersService.GetUserFromCookie(HttpContext);
            if (user == null)
                return RedirectToAction("Signin", "Users");

            return View(user);
        }

        [Route("signup")]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost("signup")]
        public IActionResult Signup(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            user.Image ??= "profile.png";
            _usersRepository.InsertUser(user);
            var _user = _usersRepository.GetUserByPhone(user.Phone!);
            _ticketsRepository.InsertUserTrainingTickets(_user.Index, _questionsRepository.GetQuestionsCount() / 20, 20);
            _cookiesService.SendUserPhoneToCookie(user.Phone!, HttpContext);
            return RedirectToAction("Index");
        }

        [Route("signin")]
        public IActionResult Signin()
        {
            return View();
        }

        [HttpPost("signin")]
        public IActionResult Signin(UserDto user)
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

        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit([FromForm] User user)
        {
            var _user = _usersService.GetUserFromCookie(HttpContext);
            if (_user == null)
                return RedirectToAction("Signin");

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            _cookiesService.UpdateUserPhoneCookie(user.Phone!, HttpContext);

            user.Index = _user.Index;
            user.Image = SaveUserImage(user.ImageFile!);

            _usersRepository.UpdateUser(user);
            return RedirectToAction("Index");
        }

        private string? SaveUserImage(IFormFile userImageFile)
        {
            if (userImageFile == null)
                return "profile.png";

            var imagePath = Guid.NewGuid().ToString("N") + Path.GetExtension(userImageFile.FileName);

            var ms = new MemoryStream();
            userImageFile.CopyTo(ms);
            System.IO.File.WriteAllBytes(Path.Combine("wwwroot", "Profile", imagePath), ms.ToArray());

            return imagePath;
        }
    }
}
