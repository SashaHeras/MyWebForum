using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Extensions;
using MyWebForum.Data;

namespace MyWebForum.Pages.User
{
    [BindProperties]
    public class ChangePassModel : PageModel
    {
        private readonly MyForumContext _db;

        public string? _pass;

        public bool IsAdmin { get; set; }

        public String? oldPass { get; set; }

        public String? newPass { get; set; }

        public String? newPassConfign { get; set; }

        public ChangePassModel(MyForumContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            IsAdmin = HttpContext.Session.Get<Models.User>("user").IsAdmin;
        }

        public IActionResult OnPost()
        {
            ModelState.Clear();

            oldPass = Request.Form["oldPass"].ToString();
            newPass = Request.Form["newPass"].ToString();
            newPassConfign = Request.Form["newPassConfign"].ToString();
            _pass = HttpContext.Session.Get<Models.User>("user").Password;

            if (oldPass == null || oldPass == String.Empty)
            {
                ModelState.AddModelError("oldPass", "Old password is empty!");
            }
            else if(oldPass.Equals(_pass) == false)
            {
                ModelState.AddModelError("oldPass", "Old password is uncorrect!");
            }
            if (newPass == null || newPass == String.Empty)
            {
                ModelState.AddModelError("newPass", "New password is empty!");
            }
            if (newPassConfign == null || newPassConfign == String.Empty)
            {
                ModelState.AddModelError("newPassConfign", "New password confign is empty!");
            }
            else if(newPass != null || newPass != String.Empty)
            {
                if(newPass != newPassConfign)
                {
                    ModelState.AddModelError("newPassConfign", "New password confign is not like new password!");
                }
                else if(newPass == oldPass)
                {
                    ModelState.AddModelError("newPass", "New password can`t be like old password!");
                }
            }
            if(ModelState.IsValid)
            {
                Models.User u = HttpContext.Session.Get<Models.User>("user");
                u.Password = newPass;

                _db.User.Update(u);
                _db.SaveChanges();

                HttpContext.Session.Set<Models.User>("user", u);

                return RedirectToPage("Index");
            }

            return Page();
        }
    }
}
