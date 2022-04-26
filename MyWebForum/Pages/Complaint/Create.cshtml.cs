using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Extensions;
using MyWebForum.Data;
using MyWebForum.Data.Interfaces;
using MyWebForum.Data.Repository.Repositories;

namespace MyWebForum.Pages.Complaint
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly MyForumContext _db;
        private IPostRepository _posts;

        public MyWebForum.Models.Complaint Complaint { get; set; }

        public bool IsAdmin { get; set; }

        public int PostId { get; set; }

        public CreateModel(MyForumContext db)
        {
            _db = db;
            _posts = new PostRepository(db);
        }

        public void OnGet(int id)
        {
            IsAdmin = HttpContext.Session.Get<MyWebForum.Models.User>("user").IsAdmin;
            PostId = id;
        }

        public IActionResult OnPost()
        {
            ModelState.Clear();
            if (Complaint.Description == null || Complaint.Description.Length == 0)
            {
                ModelState.AddModelError("Complaint.Description", "Complaint can`t be empty!");
            }
            if (ModelState.IsValid)
            {
                Complaint.PostId = PostId;

                _db.Complaint.Add(Complaint);
                _db.SaveChanges();

                return RedirectToPage("/Post/Index", new { id = PostId });
            }

            return Page();
        }
    }
}
