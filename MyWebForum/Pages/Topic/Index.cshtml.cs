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
    public class IndexModel : PageModel
    {
        private readonly MyForumContext _db;
        private IPostRepository _posts;
  
        public IEnumerable<MyWebForum.Models.Post> Posts { get; set; }

        public bool IsAdmin { get; set; }

        public int TopicId { get; set; }
        public string TopicName { get; set; }

        public int SortType { get; set; }

        public string Search { get; set; }

        public IndexModel(MyForumContext db)
        {
            _db = db;
            _posts = new PostRepository(db);
        }

        public void OnGet(int id)
        {
            IsAdmin = HttpContext.Session.Get<MyWebForum.Models.User>("user").IsAdmin;
            Posts = _posts.GetAllowedPostsByTopicId(id);
            TopicName = _db.Topic.Where(t => t.TopicId == id).FirstOrDefault().TopicName;
            TopicId = id;
        }

        public async Task<IActionResult> OnPost()
        {
            SortType = Convert.ToInt32(Request.Form["SortType"]);

            if(SortType == 1)
            {
                Posts = _posts.GetAllowedPostsByTopicId(TopicId).OrderByDescending(p => p.Views);
            }
            else if(SortType == 2)
            {
                Posts = _posts.GetAllowedPostsByTopicId(TopicId).OrderByDescending(p => p.Updated);
            }
            else if(SortType == 0)
            {
                Posts = _posts.GetAllowedPostsByTopicId(TopicId);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSearch()
        {
            ModelState.Clear();
            if(Search == null || Search.Length == 0)
            {
                ModelState.AddModelError("Search", "Enter name to search!");
            }

            if(ModelState.IsValid)
            {
                Posts = _posts.GetAllowedPostsByTopicId(TopicId).Where(p => p.PostName.Contains(Search) == true);

                return Page();
            }

            Posts = _posts.GetAllowedPostsByTopicId(TopicId);

            return Page();
        }
    }
}
