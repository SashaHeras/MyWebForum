using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebForum.Data;
using MyWebForum.Data.Interfaces;

namespace MyWebForum.Pages.Admin
{
    public class TopicsListModel : PageModel
    {
        private MyForumContext _db;
        private ITopicRepository _topics;

        public bool IsAdmin { get; set; }

        public IEnumerable<Models.Topic> Topics { get; set; }

        public void OnGet()
        {
        }
    }
}
