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
    public class TopicsListModel : PageModel
    {
        private MyForumContext _db;
        private ITopicRepository _topics;

        public bool IsAdmin { get; set; }

        public IEnumerable<Models.Topic> Topics { get; set; }

        public TopicsListModel(MyForumContext db)
        {
            _db = db;
            _topics = new TopicRepository(db);
        }

        public void OnGet()
        {
            Topics = _topics.GetAll();

            IsAdmin = HttpContext.Session.Get<Models.User>("user").IsAdmin;
        }

        public IActionResult OnPostDisallow(int id)
        {
            Models.Topic topic = _topics.GetTopicById(id);
            topic.IsAllow = false;

            _db.Topic.Update(topic);
            _db.SaveChanges();

            return RedirectToPage("TopicsList");
        }

        public IActionResult OnPostAllow(int id)
        {
            Models.Topic topic = _topics.GetTopicById(id);
            topic.IsAllow = true;

            _db.Topic.Update(topic);
            _db.SaveChanges();

            return RedirectToPage("TopicsList");
        }

        public IActionResult OnPostDelete(int id)
        {
            Models.Topic topic = _topics.GetTopicById(id);

            _db.Topic.Remove(topic);
            _db.SaveChanges();

            return RedirectToPage("TopicsList");
        }
    }
}
