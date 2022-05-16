using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Extensions;
using MyWebForum.Data;
using MyWebForum.Data.Interfaces;
using MyWebForum.Data.Repository.Repositories;

namespace MyWebForum.Pages.Comment
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly MyForumContext _db;
        private IPostRepository _posts;
        private ICommentRepository _comments;
        private IUserRepository _users;

        public MyWebForum.Models.Post? Post { get; set; }

        public MyWebForum.Models.Comment Comment { get; set; }

        public bool IsAdmin { get; set; }

        public int UserId { get; set; }

        public CreateModel(MyForumContext db)
        {
            _db = db;
            _posts = new PostRepository(db);
            _comments = new CommentRepository(db);
            _users = new UserRepository(db);
        }

        public void OnGet(int id)
        {
            IsAdmin = HttpContext.Session.Get<MyWebForum.Models.User>("user").IsAdmin;
            Post = _posts.GetPostById(id);
            UserId = HttpContext.Session.Get<MyWebForum.Models.User>("user").Id;
        }

        public IActionResult OnPost()
        {
            ModelState.Clear();
            if (Comment.CommentText == null || Comment.CommentText.Length == 0)
            {
                ModelState.AddModelError("Comment.CommentText", "Comment can`t be empty!");
            }
            if (ModelState.IsValid)
            {
                Comment.IsAllow = false;
                Comment.UserId = UserId;
                Comment.PostId = Post.PostId;
                Comment.UserName = HttpContext.Session.Get<MyWebForum.Models.User>("user").Name;

                Models.Notification n = new Models.Notification()
                {
                    Title = $"User {_users.GetUserNameById(UserId)}, leave the comment to your post {Post.PostName}",
                    Description = Comment.CommentText,
                    Checked = false,
                    Date = DateTime.Now,
                    UserId = Post.UserId
                };

                _db.Notification.Add(n);

                _db.Comment.Add(Comment);
                _db.SaveChanges();

                return RedirectToPage("/Post/Index", new {id = Post.PostId});
            }

            return Page();
        }
    }
}
