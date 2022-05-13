using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Extensions;
using MyWebForum.Data;
using MyWebForum.Data.Interfaces;
using MyWebForum.Data.Repository.Repositories;
using System.Text.RegularExpressions;

namespace MyWebForum.Pages.User
{
    [BindProperties]
    public class ChangeOwnDataModel : PageModel
    {
        private readonly MyForumContext _db;
        private IUserRepository _users;

        public MyWebForum.Models.User User { get; set; }

        public ChangeOwnDataModel(MyForumContext db)
        {
            _db = db;
            _users = new UserRepository(db);
        }

        public void OnGet()
        {
            User = HttpContext.Session.Get<Models.User>("user");
        }

        public async Task<IActionResult> OnPost()
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
                _db.User.Update(User);
                await _db.SaveChangesAsync();

                HttpContext.Session.Set<Models.User>("user", User);

                TempData["Success"] = "You data updated successfuly!";

                return RedirectToPage("Index");
            }

            return Page();
        }
    }
}
