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
    public class EditModel : PageModel
    {
        private MyForumContext _db;
        private IQuestionRepository _questions;
        private IPollRepository _poll;
        private IAnswerRepository _answer;

        public bool IsAdmin { get; set; }

        public int UserId { get; set; }

        public int CountQuestions { get; set; }

        public Models.Poll Poll { get; set; }

        public IEnumerable<Models.PollQuestion> Questions { get; set; }

        public EditModel(MyForumContext db)
        {
            _db = db;
            _questions = new QuestionRepository(db);
            _poll = new PollRepository(db);
            _answer = new AnswerRepository(db);
        }

        public void OnGet(int id)
        {
            CountQuestions = _questions.GetByPollId(id).Count();
            UserId = HttpContext.Session.Get<Models.User>("user").Id;
            IsAdmin = HttpContext.Session.Get<Models.User>("user").IsAdmin;

            Poll = _poll.GetPollById(id);

            Questions = _questions.GetByPollId(id);
        }

        public IActionResult OnPostEditpoll()
        {
            if (ModelState.IsValid)
            {
                _db.Polls.Update(Poll);
                _db.SaveChanges();
            }

            Questions = _questions.GetByPollId(Poll.Id);

            return RedirectToPage("Edit", new { id = Poll.Id });
        }

        public IActionResult OnPostAddquestion()
        {
            int id = Convert.ToInt32(Request.Form["id"]);
            Models.PollQuestion q = new Models.PollQuestion()
            {
                Name = "",
                PollId = id, 
                Saved = false
            };

            _db.PollQuestions.Add(q);
            _db.SaveChanges();

            Poll = _poll.GetPollById(id);

            Questions = _questions.GetByPollId(id);

            return Page();
        }

        public IActionResult OnPostEditquestions()
        {
            Questions = _questions.GetByPollId(Poll.Id);

            foreach (Models.PollQuestion i in Questions)
            {
                string str = Request.Form[i.Id.ToString()];
                i.Name = str;
                i.Saved = true;

                _db.PollQuestions.Update(i);
            }

            _db.SaveChanges();

            Questions = _questions.GetByPollId(Poll.Id);

            return RedirectToPage("Edit", new { id = Poll.Id });
        }
    }
}
