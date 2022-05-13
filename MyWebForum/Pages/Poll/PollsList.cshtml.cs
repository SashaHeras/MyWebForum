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
    public class PollsListModel : PageModel
    {
        private MyForumContext _db;
        private IPollRepository _polls;

        public IEnumerable<Models.Poll> Polls { get; set; }

        public bool IsAdmin { get; set; }

        public PollsListModel(MyForumContext db)
        {
            _db = db;
            _polls = new PollRepository(db);
        }

        public void OnGet()
        {
            Polls = _polls.GetAll().Where(p => p.IsAllowed == true);
            IsAdmin = HttpContext.Session.Get<Models.User>("user").IsAdmin;
        }
    }
}
