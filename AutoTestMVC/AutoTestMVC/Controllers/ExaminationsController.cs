using AutoTestMVC.Models;
using AutoTestMVC.Repositories;
using AutoTestMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoTestMVC.Controllers
{
    public class ExaminationsController : Controller
    {
        private readonly QuestionsRepository _questionsRepository;
        private readonly UsersService _usersService;
        private readonly TicketsRepository _ticketsRepository;
        private const int TicketQuestionsCount = 20;

        public ExaminationsController()
        {
            _questionsRepository = new QuestionsRepository();
            _usersService = new UsersService();
            _ticketsRepository = new TicketsRepository();
        }

        public IActionResult Index()
        {
            var user = _usersService.GetUserFromCookie(HttpContext);
            if (user == null)
                return RedirectToAction("Signin", "Users");

            var ticket = CreateRandomTicket(user);

            return View(ticket);
        }

        private Ticket CreateRandomTicket(User user)
        {
            var ticketsCount = _questionsRepository.GetQuestionsCount() / TicketQuestionsCount;
            var ticketIndex = new Random().Next(0, ticketsCount);
            var ticket = new Ticket(user.Index, ticketIndex * TicketQuestionsCount + 1, TicketQuestionsCount, 0, false);

            _ticketsRepository.InsertTicket(ticket);

            ticket.Id = _ticketsRepository.GetLastRowId();

            return ticket;
        }

        public IActionResult Exam(int ticketId, int? questionId = null, int? choiceId = null)
        {
            var user = _usersService.GetUserFromCookie(HttpContext);
            if (user == null)
                return RedirectToAction("Signin", "Users");

            var ticket = _ticketsRepository.GetTicketById(ticketId, user.Index);
            questionId = questionId ?? ticket.FromIndex;

            if (ticket.FromIndex <= questionId && ticket.FromIndex + ticket.QuestionsCount > questionId)
            {
                ViewBag.ticket = ticket;
                ViewBag.questionId = questionId;

                var question = _questionsRepository.GetQuestionById(questionId.Value);
                var _ticketData = _ticketsRepository.GetTicketDataByQuestionId(ticketId, questionId.Value);

                ViewBag.ticketDatas = _ticketsRepository.GetTicketDatasById(ticket.Id);

                if (_ticketData != null)
                {
                    ViewBag.choiceId = _ticketData.ChoiceId;
                    ViewBag.answer= _ticketData.Answer;
                }
                else if (choiceId != null)
                {
                    var answer = question.Choices!.First(choice => choice.Id == choiceId).Answer;
                    var ticketData = new TicketData(ticketId, questionId.Value, choiceId.Value, answer);

                    _ticketsRepository.InsertTicketData(ticketData);

                    ViewBag.choiceId = choiceId.Value;
                    ViewBag.answer = answer;

                    if (answer)
                    {
                        _ticketsRepository.UpdateTicketCorrectCount(ticket.Id);
                    }

                    if (ticket.QuestionsCount == _ticketsRepository.GetTicketAnswersCount(ticket.Id))
                    {
                        return RedirectToAction("ExamResult", new {ticketId = ticket.Id});
                    }
                }

                return View(question);
            }


            return NotFound();
        }

        public IActionResult GetQuestionById(int questionId = 1)
        {
            return View(_questionsRepository.GetQuestionById(questionId));
        }

        public IActionResult ExamResult(int ticketId)
        {
            var user = _usersService.GetUserFromCookie(HttpContext);
            if (user == null)
                return RedirectToAction("Signin", "Users");

            var ticket = _ticketsRepository.GetTicketById(ticketId, user.Index);


            return View(ticket);
        }
    }
}
