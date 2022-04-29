using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebForum.Data;
using MyWebForum.Data.Interfaces;
using MyWebForum.Data.Repository.Repositories;

namespace MyWebForum.Pages.User
{
    [BindProperties]
    public class UserPostsModel : PageModel
    {
        private MyForumContext _db;
        private IUserRepository _users;
        private IMarkRepository _marks;

        public string? UserName { get; set; }

        public bool IsAdmin { get; set; }

        public double AverageMark { get; set; }

        public IEnumerable<Models.Post> Posts { get; set; }

        public UserPostsModel(MyForumContext db)
        {
            _db = db;
            _users = new UserRepository(db);
            _marks = new MarkRepository(db);
        }

        public void OnGet(int id)
        {
            IsAdmin = _users.GetUserById(id).IsAdmin;
            UserName = _users.GetUserById(id).Name;
            AverageMark = _marks.GetAverageUserPostMark(id);
            Posts = _db.Post.Where(p => p.UserId == id && p.IsAllow == true);
        }
    }
}
