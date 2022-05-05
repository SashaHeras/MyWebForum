using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebForum.Data;

namespace MyWebForum.Pages.Poll
{
    [BindProperties]
    public class SetCountOfQuestionsModel : PageModel
    {
        public bool IsAdmin { get; set; }

        public int Count { get; set; } 

        public SetCountOfQuestionsModel(MyForumContext db)
        {
            
        }

        public void OnGet()
        {
            
        }

        public IActionResult OnPost()
        {
            if (Count <= 0) 
            {
                ModelState.AddModelError("Count", "Count can`t be less than 0!");
            }
            if (ModelState.IsValid)
            {
                return RedirectToPage("Create", new { count = Count });
            }

            return Page();
        }
    }
}
