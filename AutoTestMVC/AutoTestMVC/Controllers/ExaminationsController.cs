using AutoTestMVC.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AutoTestMVC.Controllers
{
    public class ExaminationsController : Controller
    {
        private QuestionsRepository _questionsRepository;

        public ExaminationsController()
        {
            _questionsRepository = new QuestionsRepository();
        }

        public IActionResult Index()
        {
            ViewBag.QuestionsCount = _questionsRepository.GetQuestionsCount();
            ViewBag.Ticket = _questionsRepository.GetQuestionsRange(1, 20);
            return View();
        }

        public IActionResult GetQuestionById(int questionId = 1)
        {
            return View(_questionsRepository.GetQuestionById(questionId));
        }
    }
}
