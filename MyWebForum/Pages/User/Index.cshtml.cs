using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Extensions;
using MyWebForum.Data;
using MyWebForum.Data.Repository.Repositories;
using MyWebForum.Models;

namespace MyWebForum.Pages.User
{
    public class IndexModel : PageModel
    {
        private readonly MyForumContext _db;
        private NotificationRepository _notif;

        public MyWebForum.Models.User User { get; set; }

        public IEnumerable<Models.Post> Posts { get; set; }

        public IFormFile Picture { get; set; }

        public int NewNotifications { get; set; }

        public IndexModel(MyForumContext db)
        {
            _db = db;
            _notif = new NotificationRepository(db);
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.Keys.FirstOrDefault(k => k == "user") == null)
            {
                return RedirectToPage("Login");
            }

            User = HttpContext.Session.Get<MyWebForum.Models.User>("user");
            Posts = _db.Post.Where(p => p.UserId == User.Id);
            NewNotifications = _notif.GetUsersUnreadedNotificationCount(User.Id);

            if (User.Picture == null)
            {
                string imagepath = Path.GetFullPath(@"wwwroot\images\default.png");
                FileStream fs = new FileStream(imagepath, FileMode.Open);
                byte[] byData = new byte[fs.Length];
                fs.Read(byData, 0, byData.Length);

                var base64 = Convert.ToBase64String(byData);
                TempData["Picture"] = String.Format("data:image/jpg;base64,{0}", base64);

                fs.Close();
            }
            else
            {
                var base64 = Convert.ToBase64String(User.Picture);
                TempData["Picture"] = String.Format("data:image/jpg;base64,{0}", base64);
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostUpload()
        {
            if(Picture == null)
            {
                ModelState.AddModelError("Picture", "Put a picture");
            }
            if (ModelState.IsValid)
            {
                byte[] imageData = null;

                using (var binaryReader = new BinaryReader(Picture.OpenReadStream()))
                {
                    long f = Picture.Length;
                    int c = Convert.ToInt32(Picture.Length);
                    imageData = binaryReader.ReadBytes(c);
                }

                User.Picture = imageData;

                _db.User.Update(User);
                _db.SaveChanges();

                HttpContext.Session.Remove("user");
                HttpContext.Session.Set<Models.User>("user", User);

                TempData["Success"] = "You picture added successfuly!";

                return RedirectToPage("Index");
            }

            return RedirectToPage("Index");
        }
    }
}
