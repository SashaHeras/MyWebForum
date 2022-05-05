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
    public class CreateModel : PageModel
    {
        private MyForumContext _db;
        private IQuestionRepository _questions;
        private IPollRepository _poll;
        private IAnswerRepository _answer;

        public bool IsAdmin { get; set; }

        public int UserId { get; set; }

        public int CountQuestions { get; set; }

        public Models.Poll Poll { get; set; }

        public IEnumerable<int> Numbers { get; set; }

        public CreateModel(MyForumContext db)
        {
            _db = db;
            _questions = new QuestionRepository(db);
            _poll = new PollRepository(db);
            _answer = new AnswerRepository(db);
        }

        public void OnGet(int count)
        {
            CountQuestions = count;
            UserId = HttpContext.Session.Get<Models.User>("user").Id;

            Poll = new Models.Poll()
            {
                Id = _poll.GetAll().OrderBy(p => p.Id).LastOrDefault().Id + 1,
                CountQuestions = count,
                UserId = UserId
            };    
            
            List<int> numbers = new List<int>(CountQuestions);

            for (int i = 1; i <= CountQuestions; i++)
            {
                numbers.Add(i);
            }

            Numbers = numbers.AsQueryable();
        }

        public IActionResult OnPost()
        {
            if(ModelState.IsValid)
            {
                _db.Polls.Add(Poll);
                _db.SaveChanges();

                for (int i = 1; i <= CountQuestions; i++)
                {
                    Models.PollQuestion question = new Models.PollQuestion()
                    {
                        Name = Request.Form[i.ToString()],
                        PollId = Poll.Id
                    };

                    _db.PollQuestions.Add(question);
                }
                
                _db.SaveChanges();

                return RedirectToPage("PollsList");
            }

            return Page();
        }
    }
}
