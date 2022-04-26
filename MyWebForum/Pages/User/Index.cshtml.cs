using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Extensions;
using MyWebForum.Data;
using MyWebForum.Models;

namespace MyWebForum.Pages.User
{
    public class IndexModel : PageModel
    {
        private readonly MyForumContext _db;

        public MyWebForum.Models.User User { get; set; }

        public IEnumerable<MyWebForum.Models.Post> Posts { get; set; }

        public IndexModel(MyForumContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            User = HttpContext.Session.Get<MyWebForum.Models.User>("user");
            Posts = _db.Post.Where(p => p.UserId == User.Id);

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
        }

        public async Task<IActionResult> OnPost()
        {
            return Page();
        }
    }
}
