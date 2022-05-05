using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Extensions;
using MyWebForum.Data;
using MyWebForum.Data.Interfaces;
using MyWebForum.Data.Repository.Repositories;

namespace MyWebForum.Pages.Poll
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private MyForumContext _db;
        private IPollRepository _polls;
        private IQuestionRepository _questions;
        private IAnswerRepository _answer;

        public Models.Poll Poll { get; set; }

        public IEnumerable<Models.PollQuestion> Questions { get; set; }

        public bool IsAdmin { get; set; }

        public int UserId { get; set; }

        public int UserAnswer { get; set; }

        public IndexModel(MyForumContext db)
        {
            _db = db;
            _polls = new PollRepository(db);
            _questions = new QuestionRepository(db);
            _answer = new AnswerRepository(db);
        }

        public void OnGet(int id)
        {
            Poll = _polls.GetPollById(id);
            Questions = _questions.GetByPollId(id);
            Poll.CountQuestions = Questions.Count();
            IsAdmin = HttpContext.Session.Get<Models.User>("user").IsAdmin;
            UserId = HttpContext.Session.Get<Models.User>("user").Id;
            if(_answer.GetByUserIdAndPollId(UserId, id) == null)
            {
                UserAnswer = -1;
            }
            else
            {
                UserAnswer = _answer.GetByUserIdAndPollId(UserId, id).QuestionId;
            }

            Poll.CountViews++;

            _db.SaveChanges();
        }

        public IActionResult OnPostAnswer(int id)
        {
            int poll_id = Convert.ToInt32(Request.Form["PollId"]);

            if (_answer.GetByUserIdAndPollId(UserId, poll_id) == null)
            {
                Models.UserPollAnswer a = new Models.UserPollAnswer()
                {
                    UserId = UserId,
                    QuestionId = id
                };

                _db.UsersPollsAnswers.Add(a);
                _db.SaveChanges();

                UpdateCountAnswers(poll_id);
            }
            else
            {
                Models.UserPollAnswer a = _answer.GetByUserIdAndPollId(UserId, poll_id);
                a.QuestionId = id;

                _db.UsersPollsAnswers.Update(a);
                _db.SaveChanges();

                UpdateCountAnswers(poll_id);
            }

            return RedirectToPage("Index", new { id = Convert.ToInt32(Request.Form["PollId"]) });
        }

        public IActionResult OnPostDelete(int id)
        {
            Questions = _questions.GetByPollId(id);

            foreach(Models.PollQuestion i in Questions)
            {
                List<Models.UserPollAnswer> ans = _answer.GetByQuestionId(i.Id).ToList();

                foreach(Models.UserPollAnswer j in ans)
                {
                    _db.UsersPollsAnswers.Remove(j);
                }

                _db.PollQuestions.Remove(i);
            }

            Poll = _polls.GetPollById(id);

            _db.Polls.Remove(Poll);
            _db.SaveChanges();

            return RedirectToPage("PollsList");
        }

        public void UpdateCountAnswers(int id)
        {
            IEnumerable<Models.PollQuestion> questions = _questions.GetByPollId(id);

            foreach (Models.PollQuestion i in questions)
            {
                i.CountAnswers = _answer.CountAnswersOnQuestion(i.Id);

                _db.PollQuestions.Update(i);
            }

            _db.SaveChanges();
        }
    }
}
