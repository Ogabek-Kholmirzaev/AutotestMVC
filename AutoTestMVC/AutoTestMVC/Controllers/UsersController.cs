using AutoTestMVC.Models;
using AutoTestMVC.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AutoTestMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly UsersRepository _usersRepository;

        public UsersController()
        {
            _usersRepository = new UsersRepository();
        }

        public IActionResult Index()
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
            return RedirectToAction("Index");
        }
    }
}
