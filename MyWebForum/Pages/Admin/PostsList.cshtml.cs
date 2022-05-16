using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Extensions;
using MyWebForum.Data;
using MyWebForum.Data.Interfaces;
using MyWebForum.Data.Repository.Repositories;

namespace MyWebForum.Pages.Admin
{
    public class PostsListModel : PageModel
    {
        private MyForumContext _db;
        private IPostRepository _posts;

        public IEnumerable<Models.Post> Posts { get; set;}

        public bool IsAdmin { get; set; }

        public PostsListModel(MyForumContext db)
        {
            _db = db;
            _posts = new PostRepository(db);
        }

        public void OnGet()
        {
            Posts = _posts.GetAll();
            IsAdmin = HttpContext.Session.Get<Models.User>("user").IsAdmin;
        }

        public IActionResult OnPostDisallow(int id)
        {
            Models.Post post = _posts.GetPostById(id);
            post.IsAllow = false;

            _db.Post.Update(post);
            _db.SaveChanges();

            return RedirectToPage("/Admin/CreateNotification", new { ui = post.UserId, t = "Your post was disallowed by administrator", route = "/Admin/PostsList" });
        }

        public IActionResult OnPostAllow(int id)
        {
            Models.Post post = _posts.GetPostById(id);
            post.IsAllow = true;

            _db.Post.Update(post);
            _db.SaveChanges();

            return RedirectToPage("/Admin/CreateNotification", new { ui = post.UserId, t = "Your post was allowed by administrator", route = "/Admin/PostsList" });
        }

        public IActionResult OnPostDelete(int id)
        {
            Models.Post post = _posts.GetPostById(id);

            _db.Post.Remove(post);
            _db.SaveChanges();

            return RedirectToPage("/Admin/CreateNotification", new { ui = post.UserId, t = "Your post was deleted by administrator", route = "/Admin/PostsList" });
        }
    }
}
