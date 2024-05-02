using System;
using System.Linq;
using System.Web.Mvc;
using Encryption.DB;

namespace Encryption.Controllers
{
    public class SurveyController : Controller
    {
        private static AllQuestions _allQuestions = new AllQuestions();

        // GET: Survey/Question/1
        public ActionResult Survey(int id = 1)
        {
            var question = _allQuestions.Questions.FirstOrDefault(q => q.QuestionId == id);
            if (question == null)
                return RedirectToAction("Completed"); // Перенаправление на страницу завершения, если вопросов больше нет
            return View(question);
        }

        [HttpPost]
        public ActionResult Question(QModel model)
        {
            var question = _allQuestions.Questions.FirstOrDefault(q => q.QuestionId == model.QuestionId);
            if (question != null)
            {
                question.UserAnswer = model.UserAnswer;
            }

            var nextQuestionId = model.QuestionId + 1;
            var nextQuestion = _allQuestions.Questions.FirstOrDefault(q => q.QuestionId == nextQuestionId);
            if (nextQuestion == null)
                return RedirectToAction("Completed"); // Перенаправление на страницу завершения

            return RedirectToAction("Survey", new { id = nextQuestionId });
        }

        public ActionResult Completed()
        {
            int totalQuestions = _allQuestions.Questions.Count;
            int correctAnswers = _allQuestions.Questions.Count(q => q.CorrectAnswer == q.UserAnswer);
            double percentage = (double)correctAnswers / totalQuestions * 100;

            ViewBag.Percentage = Math.Round(percentage, 2); // Округление до двух знаков после запятой

            return View();
        }
        public ActionResult Advice()
        {
            var incorrectAnswers = _allQuestions.Questions.Where(q => q.UserAnswer.HasValue && q.UserAnswer.Value != q.CorrectAnswer).ToList();
            return View(incorrectAnswers);
        }

    }
}
