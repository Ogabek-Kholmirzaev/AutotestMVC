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
            return View();
        }

        public IActionResult GetQuestionById()
        {
            return View();
        }
    }
}
