using AutoTestMVC.Models;
using AutoTestMVC.Repositories;
using AutoTestMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoTestMVC.Controllers
{
    public class TicketsController : Controller
    {
        private readonly UsersService _usersService;
        private readonly TicketsRepository _ticketsRepository;

        public TicketsController(UsersService usersService, TicketsRepository ticketsRepository)
        {
            _usersService = usersService;
            _ticketsRepository = ticketsRepository;
        }

        public IActionResult Index()
        {
            var user = _usersService.GetUserFromCookie(HttpContext);
            if (user == null)
                return RedirectToAction("Signin", "Users");


            var tickets = _ticketsRepository.GetTicketsByUserId(user.Index);
            var ticketsDict = 
                tickets.ToDictionary(ticket => ticket.Id, ticket => _ticketsRepository.GetTicketAnswersCount(ticket.Id));

            ViewBag.TicketsDict = ticketsDict;

            return View(tickets);
        }

        public IActionResult ShowTicket(int ticketId, int ticketNumber)
        {
            var user = _usersService.GetUserFromCookie(HttpContext);
            if (user == null)
                return RedirectToAction("Signin", "Users");

            var ticket = _ticketsRepository.GetTicketById(ticketId, user.Index);
            ViewBag.selectedAnswers = _ticketsRepository.GetTicketAnswersCount(ticket.Id);
            ViewBag.ticketNumber = ticketNumber;

            return View(ticket);
        }
    }
}
