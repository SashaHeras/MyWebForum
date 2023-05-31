using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Extensions;
using MyWebForum.Data;
using MyWebForum.Data.Interfaces;
using MyWebForum.Data.Repository.Repositories;

namespace MyWebForum.Pages.Admin
{
    [BindProperties]
    public class PollsListModel : PageModel
    {
        private MyForumContext _db;
        private IPollRepository _polls;
        private IQuestionRepository _questions;
        private IAnswerRepository _answers;

        public IEnumerable<Models.Poll> Polls { get; set; }

        public bool IsAdmin { get; set; }

        public PollsListModel(MyForumContext db)
        {
            _db = db;
            _polls = new PollRepository(db);
            _questions = new QuestionRepository(db);
            _answers = new AnswerRepository(db);
        }

        public IActionResult OnGet()
        {
            if(HttpContext.Session.Get<Models.User>("user").IsAdmin != true)
            {
                HttpContext.Session.Remove("user");

                return RedirectToPage("/User/Login");
            }

            IsAdmin = HttpContext.Session.Get<Models.User>("user").IsAdmin;

            Polls = _polls.GetAll();

            return Page();
        }

        public IActionResult OnPostDisallow(int id)
        {
            Models.Poll poll = _polls.GetPollById(id);
            poll.IsAllowed = false;

            var questions = _questions.GetByPollId(poll.Id);

            foreach (var q in questions)
            {
                q.Saved = true;
                _db.PollQuestions.Update(q);
            }

            _db.Polls.Update(poll);
            _db.SaveChanges();

            return RedirectToPage("/Admin/CreateNotification", new { ui = poll.UserId, t = "Your poll was disallowed by administrator", route = "/Admin/PollsList" });
        }

        public IActionResult OnPostAllow(int id)
        {
            Models.Poll poll = _polls.GetPollById(id);
            poll.IsAllowed = true;

            _db.Polls.Update(poll);
            _db.SaveChanges();

            return RedirectToPage("/Admin/CreateNotification", new { ui = poll.UserId, t = "Your poll was allowed by administrator", route = "/Admin/PollsList" });
        }

        public IActionResult OnPostDelete(int id)
        {
            Models.Poll poll = _polls.GetPollById(id);

            IEnumerable<Models.PollQuestion> q = _questions.GetByPollId(id);

            foreach(Models.PollQuestion i in q)
            {
                IEnumerable<Models.UserPollAnswer> a = _answers.GetByQuestionId(i.Id);

                foreach(Models.UserPollAnswer j in a)
                {
                    _db.UsersPollsAnswers.Remove(j);
                }

                _db.PollQuestions.Remove(i);
            }

            _db.Polls.Remove(poll);
            _db.SaveChanges();

            return RedirectToPage("/Admin/CreateNotification", new { ui = poll.UserId, t = "Your poll was deleted by administrator", route = "/Admin/PollsList" });
        }
    }
}
