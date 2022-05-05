using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Extensions;
using MyWebForum.Data;
using MyWebForum.Data.Interfaces;
using MyWebForum.Data.Repository.Repositories;

namespace MyWebForum.Pages.Admin
{
    public class CompaintsListModel : PageModel
    {
        private MyForumContext _db;
        private IComplaintRepository _compaints;

        public IEnumerable<Models.Complaint> Complaints { get; set; }

        public bool IsAdmin { get; set; }

        public CompaintsListModel(MyForumContext db)
        {
            _db = db;
            _compaints = new ComplaintRepository(db);
        }

        public void OnGet()
        {
            IsAdmin = HttpContext.Session.Get<Models.User>("user").IsAdmin;
            Complaints = _compaints.GetAll();
        }

        public IActionResult OnPostDelete(int id)
        {
            Models.Complaint com = _compaints.GetComplaintById(id);

            _db.Complaint.Remove(com);
            _db.SaveChanges();

            return RedirectToPage("UsersList");
        }
    }
}
