using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebForum.Data;
using MyWebForum.Data.Interfaces;
using MyWebForum.Data.Repository.Repositories;

namespace MyWebForum.Pages.Question
{
    public class DeleteModel : PageModel
    {
        private MyForumContext _db;
        private IQuestionRepository _questions;
        private IAnswerRepository _answer;
        private IPostRepository _post;

        public DeleteModel(MyForumContext db)
        {
            _db = db;
            _answer = new AnswerRepository(db);
            _post = new PostRepository(db);
            _questions = new QuestionRepository(db);
        }

        public IActionResult OnGet(int id)
        {
            Models.PollQuestion question = _questions.GetQuestionById(id);

            int poll = _questions.GetQuestionById(id).PollId;

            List<Models.UserPollAnswer> ans = _answer.GetByQuestionId(id).ToList();

            foreach(Models.UserPollAnswer answer in ans)
            {
                _db.UsersPollsAnswers.Remove(answer);
            }
            _db.SaveChanges();

            _db.PollQuestions.Remove(question);
            _db.SaveChanges();

            return RedirectToPage("/Poll/Edit", new { id = poll });
        }
    }
}
