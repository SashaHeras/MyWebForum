using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Extensions;
using MyWebForum.Data;
using MyWebForum.Data.Interfaces;
using MyWebForum.Data.Repository.Repositories;

namespace MyWebForum.Pages.Post
{
    [BindProperties]
    public class SearchModel : PageModel
    {
        private MyForumContext _db;
        private IPostRepository _posts;
        private ITopicRepository _topics;

        public bool IsAdmin { get; set; }

        private IEnumerable<Models.Post> Posts { get; set; }

        public IEnumerable<SearchPost> SearchedPosts { get; set; }

        public string Search { get; set; }

        public SearchModel(MyForumContext db)
        {
            _db = db;
            _posts = new PostRepository(db);
            _topics = new TopicRepository(db);
        }

        public void OnGet(string search)
        {
            Search = search;
            IsAdmin = HttpContext.Session.Get<Models.User>("user").IsAdmin;

            Posts = _posts.GetAll().Where(p => p.IsAllow == true);

            List<SearchPost> list = new List<SearchPost>();

            foreach(Models.Post post in Posts)
            {
                if (post.PostName.Contains(Search) == true || post.Description.Contains(Search)) 
                {
                    list.Add(new SearchPost(post, _topics.GetTopicById(post.TopicId).TopicName));
                }
            }

            SearchedPosts = list.AsQueryable();
        }

        public IActionResult OnPostSearch()
        {
            if(Search == null || Search == String.Empty)
            {
                ModelState.AddModelError("Search", "Search line is empty!");
            }
            else
            {
                Search = Search;

                Posts = _posts.GetAll().Where(p => p.IsAllow == true);

                List<SearchPost> list = new List<SearchPost>();

                foreach (Models.Post post in Posts)
                {
                    if (post.PostName.Contains(Search) == true || post.Description.Contains(Search))
                    {
                        list.Add(new SearchPost(post, _topics.GetTopicById(post.TopicId).TopicName));
                    }
                }

                SearchedPosts = list.AsQueryable();
            }

            return Page();
        }
    }

    public class SearchPost : Models.Post
    {
        public string TopicName { get; set; }

        public SearchPost(Models.Post post, string name)
        {
            this.PostId = post.PostId;
            this.PostName = post.PostName;
            this.TopicName = name;
            this.TopicId = post.TopicId;
        }
    }
}
