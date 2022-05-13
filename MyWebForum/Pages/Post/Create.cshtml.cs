using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Extensions;
using MyWebForum.Data;
using MyWebForum.Data.Interfaces;

namespace MyWebForum.Pages.Post
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly MyForumContext _db;
        private IPostRepository _posts;
        private IMarkRepository _marks;
        private ICommentRepository _comments;
        private IUserRepository _users;

        public MyWebForum.Models.Post? Post { get; set; }

        public bool IsAdmin { get; set; }

        public CreateModel(MyForumContext db)
        {
            _db = db;
        }

        public void OnGet(int id)
        {
            Post = new Models.Post()
            {
                TopicId = id,
                UserId = HttpContext.Session.Get<MyWebForum.Models.User>("user").Id
            };
            IsAdmin = HttpContext.Session.Get<MyWebForum.Models.User>("user").IsAdmin;
        }

        public IActionResult OnPostAdd()
        {
            ModelState.Clear();
            if (Post.PostName == null || Post.PostName.Length == 0)
            {
                ModelState.AddModelError("Post.PostName", "Post name can`t be empty!");
            }
            if (Post.Description == null || Post.Description.Length == 0)
            {
                ModelState.AddModelError("Post.Description", "Post description can`t be empty!");
            }
            if(Post.PostName == Post.Description)
            {
                ModelState.AddModelError("Post.Description", "Post description can`t be like post name!");
            }
            if(ModelState.IsValid)
            {
                Post.Views = 0;
                Post.Updated = DateTime.Today;
                Post.IsAllow = false;

                _db.Post.Add(Post);
                _db.SaveChanges();

                TempData["Success"] = "Post created successfuly!";

                return RedirectToPage("Index", new { id = Post.PostId });
            }

            return Page();
        }
    }
}
