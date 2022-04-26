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
    public class IndexModel : PageModel
    {
        private readonly MyForumContext _db;
        private IPostRepository _posts;
        private IMarkRepository _marks;
        private ICommentRepository _comments;
        private IUserRepository _users;

        public MyWebForum.Models.Post? Post { get; set; }

        public IEnumerable<MyWebForum.Models.Comment> Comments { get; set; }

        public bool IsAdmin { get; set; }

        public int CurrentUserId { get; set; }

        public string? AuthorName { get; set; } = null!;

        public int Mark { get; set; }

        public IndexModel(MyForumContext db)
        {
            _db = db;
            _posts = new PostRepository(db);
            _marks = new MarkRepository(db);
            _comments = new CommentRepository(db);
            _users = new UserRepository(db);
        }

        public void OnGet(int id)
        {
            IsAdmin = HttpContext.Session.Get<MyWebForum.Models.User>("user").IsAdmin;
            Post = _posts.GetPostById(id);
            AuthorName = _db.User.First(u => u.Id == Post.UserId).Name;
            CurrentUserId = HttpContext.Session.Get<MyWebForum.Models.User>("user").Id;
            Mark = _marks.GetPostMark(id);
            Comments = _comments.GetAllAllowedComments(id);
        }

        public IActionResult OnPostLike()
        {
            if(_marks.GetMarkByUserAndPostId(CurrentUserId, Post.PostId) != null)
            {
                Models.UserPostMark mark = _marks.GetMarkByUserAndPostId(CurrentUserId, Post.PostId);

                if(mark.PostMark == -1)
                {
                    mark.PostMark = 1;

                    _db.Mark.Update(mark);
                }
            }
            else
            {
                Models.UserPostMark m = new Models.UserPostMark()
                {
                    PostMark = 1,
                    UserId = CurrentUserId,
                    PostId = Post.PostId
                };

                _db.Mark.Add(m);
            }
            
            _db.SaveChanges();

            return RedirectToPage("Index", new { id = Post.PostId });
        }

        [HttpPost]
        public IActionResult OnPostDislike()
        {
            if (_marks.GetMarkByUserAndPostId(CurrentUserId, Post.PostId) != null)
            {
                Models.UserPostMark mark = _marks.GetMarkByUserAndPostId(CurrentUserId, Post.PostId);

                if (mark.PostMark == 1)
                {
                    mark.PostMark = -1;

                    _db.Mark.Update(mark);
                }
            }
            else
            {
                Models.UserPostMark m = new Models.UserPostMark()
                {
                    PostMark = -1,
                    UserId = CurrentUserId,
                    PostId = Post.PostId
                };

                _db.Mark.Add(m);
            }

            _db.SaveChanges();

            return RedirectToPage("Index", new { id = Post.PostId });
        }
    }
}