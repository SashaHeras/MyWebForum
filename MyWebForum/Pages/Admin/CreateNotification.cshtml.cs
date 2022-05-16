using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Extensions;
using MyWebForum.Data;

namespace MyWebForum.Pages.Admin
{
    [BindProperties]
    public class CreateNotificationModel : PageModel
    {
        private MyForumContext _db;

        public string Route { get; set; }

        public bool IsAdmin { get; set; }

        public Models.Notification Notification { get; set; } 

        public CreateNotificationModel(MyForumContext db)
        {
            _db = db;
        }

        public void OnGet(int ui, string t, string route)
        {
            IsAdmin = HttpContext.Session.Get<Models.User>("user").IsAdmin;
            Notification = new()
            {
                UserId = ui,
                Title = t,
                Checked = false,
                Date = DateTime.Now
            };
            Route = route;
        }

        public IActionResult OnGet(int ui, string t, string d, int pi)
        {
            Notification = new()
            {
                UserId = ui,
                Title = t,
                Description = d,    
                Checked = false,
                Date = DateTime.Now
            };

            _db.Notification.Add(Notification);
            _db.SaveChanges();

            return RedirectToPage("/Post/Index", new { id = pi });
        }

        public IActionResult OnPostAdd()
        {
            if (Notification.Description != null)
            {
                _db.Notification.Add(Notification);
                _db.SaveChanges();

                return RedirectToPage(Route.ToString());
            }

            return Page();
        }
    }
}
