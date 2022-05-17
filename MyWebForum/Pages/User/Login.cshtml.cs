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
    public class LoginModel : PageModel
    {
        private readonly MyForumContext _db;
        private IUserRepository _users;

        public MyWebForum.Models.User User { get; set; }

        public LoginModel(MyForumContext db)
        {
            _db = db;
            _users = new UserRepository(db);
        }

        public void OnGet()
        {
            HttpContext.Session.Remove("user");
        }

        public async Task<IActionResult> OnPost()
        {
            ModelState.Clear();
            if (User.Email == null || User.Email == String.Empty)
            {
                ModelState.AddModelError("User.Email", "User Email can`t be empty!");
            }
            if(!Regex.IsMatch(User.Email, @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}"))
            {
                ModelState.AddModelError("User.Email", "Uncorrect syntacsis of email!");
            }
            if(User.Password == null || User.Password == String.Empty)
            {
                ModelState.AddModelError("User.Password", "User Password can`t be empty!");
            }
            if(CheckPass(User.Email, User.Password) == false)
            {
                ModelState.AddModelError("User.Password", "This password is not match!");
            }
            if (ModelState.IsValid)
            {
                HttpContext.Session.Set<MyWebForum.Models.User>("user", _users.GetUser(User.Email, User.Password));

                return RedirectToPage("../Index");
            }

            return Page();
        }

        public bool CheckPass(string email, string pass)
        {
            if (_users.GetAll().Where(u => u.Email.CompareTo(email) == 0).FirstOrDefault() != null)
            {
                if(_users.GetAll().Where(u => u.Email.CompareTo(email) == 0).FirstOrDefault().Password.CompareTo(pass) == 0)
                {
                    return true;
                }                
            }

            return false;
        }
    }
}
