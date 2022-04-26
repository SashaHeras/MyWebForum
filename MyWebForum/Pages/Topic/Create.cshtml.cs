using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Extensions;
using MyWebForum.Data;
using MyWebForum.Data.Interfaces;
using MyWebForum.Data.Repository.Repositories;

namespace MyWebForum.Pages.Topic
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly MyForumContext _db;
        private ITopicRepository _topics;

        public MyWebForum.Models.Topic Topic { get; set; }

        public bool IsAdmin { get; set; }

        public CreateModel(MyForumContext db)
        {
            _db = db;
            _topics = new TopicRepository(db);
        }

        public void OnGet()
        {
            IsAdmin = HttpContext.Session.Get<MyWebForum.Models.User>("user").IsAdmin;
        }

        public async Task<IActionResult> OnPost()
        {
            ModelState.Clear();
            if (Topic.TopicName == null || Topic.TopicName == String.Empty)
            {
                ModelState.AddModelError("Topic.TopicName", "Topic name can`t be empty!");
            }
            if (ModelState.IsValid)
            {
                Topic.IsAllow = false;

                await _db.Topic.AddAsync(Topic);
                await _db.SaveChangesAsync();

                return RedirectToPage("../Index");
            }
            return Page();
        }
    }
}
