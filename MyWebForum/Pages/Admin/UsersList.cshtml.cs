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

        public IEnumerable<Models.User> Users { get; set; }

        public bool IsAdmin { get; set; }

        public UsersListModel(MyForumContext db)
        {
            _db = db;
            _users = new UserRepository(db);
        }

        public void OnGet()
        {
            IsAdmin = HttpContext.Session.Get<Models.User>("user").IsAdmin;
            Users = _users.GetAllWithoutUser(HttpContext.Session.Get<Models.User>("user").Id);
        }

        public IActionResult OnPostPickuprights(int id)
        {
            Models.User user = _users.GetUserById(id);
            user.IsAdmin = false;

            _db.User.Update(user);
            _db.SaveChanges();

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

            _db.User.Remove(user);
            _db.SaveChanges();

            return RedirectToPage("UsersList");
        }
    }
}
