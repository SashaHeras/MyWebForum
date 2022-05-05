using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebForum.Data;
using System.Text.RegularExpressions;

namespace MyWebForum.Pages.Admin
{
    [BindProperties]
    public class CreateUserModel : PageModel
    {
        private readonly MyForumContext _db;

        public Models.User User { get; set; }

        public bool IsAdmin { get; set; }

        public CreateUserModel(MyForumContext db)
        {
            _db = db;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            ModelState.Clear();
            if (User.Email == null || User.Email == String.Empty)
            {
                ModelState.AddModelError("User.Email", "User Email can`t be empty!");
            }
            else if (!Regex.IsMatch(User.Email, @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}"))
            {
                ModelState.AddModelError("User.Email", "Uncorrect syntacsis of email!");
            }
            if (User.Password == null || User.Password == String.Empty)
            {
                ModelState.AddModelError("User.Password", "User password can`t be empty!");
            }
            if (User.Name == null || User.Name == String.Empty)
            {
                ModelState.AddModelError("User.Name", "User name can`t be empty!");
            }
            if (User.Surname == null || User.Surname == String.Empty)
            {
                ModelState.AddModelError("User.Surname", "User surname can`t be empty!");
            }
            if (User.Address == null || User.Address == String.Empty)
            {
                ModelState.AddModelError("User.Address", "User ddress can`t be empty!");
            }
            if (User.Age < 0)
            {
                ModelState.AddModelError("User.Age", "User age can`t be less than 0!");
            }
            if (ModelState.IsValid)
            {
                _db.User.Add(User);
                _db.SaveChanges();

                return RedirectToPage("UsersList");
            }

            return Page();
        }
    }
}
