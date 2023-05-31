using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Extensions;
using MyWebForum.Data;
using MyWebForum.Data.Interfaces;
using MyWebForum.Data.Repository.Repositories;

namespace MyWebForum.Pages.Admin
{
    public class UsersListModel : PageModel
    {
        private MyForumContext _db;
        private IUserRepository _users;
        private IPostRepository _posts;
        private IMarkRepository _marks;
        private IPollRepository _polls;

        public IEnumerable<Models.User> Users { get; set; }

        public bool IsAdmin { get; set; }

        public UsersListModel(MyForumContext db)
        {
            var l = Request;
            _db = db;
            _users = new UserRepository(db);
            _posts = new PostRepository(db);
            _marks = new MarkRepository(db);
            _polls = new PollRepository(db);
        }

        public void OnGet()
        {
            var l = Request;
            IsAdmin = HttpContext.Session.Get<Models.User>("user").IsAdmin;
            Users = _users.GetAllWithoutUser(HttpContext.Session.Get<Models.User>("user").Id);
        }

        public IActionResult OnPostPickuprights(int id)
        {
            try
            {
                Models.User user = _users.GetUserById(id);
                user.IsAdmin = false;

                _db.User.Update(user);
                _db.SaveChanges();
            }
            catch(Exception ex)
            {

            }

            return RedirectToPage("UsersList");
        }

        public IActionResult OnPostGiverights(int id)
        {
            Models.User user = _users.GetUserById(id);
            user.IsAdmin = true;

            _db.User.Update(user);
            _db.SaveChanges();

            return RedirectToPage("UsersList");
        }

        public IActionResult OnPostDelete(int id)
        {
            Models.User user = _users.GetUserById(id);

            var posts = _posts.GetPostsByUserId(user.Id);
            var marks = _marks.GetAll().Where(m=>m.UserId == user.Id).ToList();
            var polls = _polls.GetAll().Where(p => p.UserId == user.Id).ToList();

            foreach (var mark in marks)
            {
                _db.Mark.Remove(mark);
            }

            foreach (var post in posts)
            {
                _db.Post.Remove(post);
            }

            foreach (var poll in polls)
            {
                _db.Polls.Remove(poll);
            }

            _db.User.Remove(user);
            _db.SaveChanges();

            return RedirectToPage("UsersList");
        }
    }
}
