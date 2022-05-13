using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Extensions;
using MyWebForum.Data;
using MyWebForum.Data.Interfaces;
using MyWebForum.Data.Repository.Repositories;

namespace MyWebForum.Pages.User
{
    [BindProperties]
    public class NotificationsModel : PageModel
    {
        private MyForumContext _db;
        private INotificationInterface _notif;

        public bool IsAdmin { get; set; }
        public int CurrentUserId { get; set; }

        public IEnumerable<Models.Notification> Notifications { get; set; }

        public NotificationsModel(MyForumContext db)
        {
            _db = db;
            _notif = new NotificationRepository(db);
        }

        public void OnGet(int id)
        {
            IsAdmin = HttpContext.Session.Get<Models.User>("user").IsAdmin;
            CurrentUserId = HttpContext.Session.Get<Models.User>("user").Id;

            Notifications = _notif.GetNotificationsByUserId(id).OrderByDescending(n => n.Checked);
        }

        public IActionResult OnPostCheck(int id)
        {
            Models.Notification n = _notif.GetNotificationById(id);
            n.Checked = true;

            _db.Notification.Update(n);
            _db.SaveChanges();

            Notifications = _notif.GetNotificationsByUserId(CurrentUserId).OrderByDescending(n => n.Checked);

            return Page();
        }

        public IActionResult OnPostDelete(int id)
        {
            Models.Notification n = _notif.GetNotificationById(id);

            _db.Notification.Remove(n);
            _db.SaveChanges();

            Notifications = _notif.GetNotificationsByUserId(CurrentUserId).OrderByDescending(n => n.Checked);

            return Page();
        }
    }
}
