using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Extensions;
using MyWebForum.Data;
using MyWebForum.Data.Interfaces;
using MyWebForum.Data.Repository.Repositories;

namespace MyWebForum.Pages.Topic
{
    [BindProperties]
    public class SearchModel : PageModel
    {
        private readonly MyForumContext _db;
        private ITopicRepository _topics;

        public bool IsAdmin { get; set; }

        public IEnumerable<MyWebForum.Models.Topic> Topics { get; set; }

        public string Search { get; set; }

        public SearchModel(MyForumContext db)
        {
            _db = db;
            _topics = new TopicRepository(db);
        }

        public void OnGet()
        {
            IsAdmin = HttpContext.Session.Get<MyWebForum.Models.User>("user").IsAdmin;
        }

        public async Task<IActionResult> OnPost()
        {
            ModelState.Clear();
            if (Search == null || Search == String.Empty)
            {
                ModelState.AddModelError("Search", "Search line can`t be empty!");
            }

            if (ModelState.IsValid)
            {
                Topics = _db.Topic.Where(t => t.TopicName.Contains(Search) && t.IsAllow == true);

                return Page();
            }

            return Page();
        }
    }
}
