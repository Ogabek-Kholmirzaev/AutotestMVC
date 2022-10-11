using AutoTestMVC.Repositories;
using AutoTestMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoTestMVC.Controllers
{
    public class TicketsController : Controller
    {
        private readonly UsersService _usersService;
        private readonly TicketsRepository _ticketsRepository;

        public TicketsController()
        {
            _usersService = new UsersService();
            _ticketsRepository = new TicketsRepository();
        }

        public IActionResult Index()
        {
            var user = _usersService.GetUserFromCookie(HttpContext);
            if (user == null)
                return RedirectToAction("Signin", "Users");


            var tickets = _ticketsRepository.GetTicketsByUserId(user.Index);

            return View(tickets);
        }
    }
}
