using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Extensions;
using MyWebForum.Data;
using MyWebForum.Data.Interfaces;
using MyWebForum.Data.Repository.Repositories;

namespace MyWebForum.Pages.Admin
{
    public class CommentsListModel : PageModel
    {
        private MyForumContext _db;
        private ICommentRepository _comments;

        public IEnumerable<Models.Comment> Comments { get; set; }

        public bool IsAdmin { get; set; }

        public CommentsListModel(MyForumContext db)
        {
            _db = db;
            _comments = new CommentRepository(db);
        }

        public void OnGet()
        {
            Comments = _comments.GetAll();
            IsAdmin = HttpContext.Session.Get<Models.User>("user").IsAdmin;
        }

        public IActionResult OnPostDisallow(int id)
        {
            Models.Comment com = _comments.GetCommentById(id);
            com.IsAllow = false;

            _db.Comment.Update(com);
            _db.SaveChanges();

            return RedirectToPage("CommentsList");
        }

        public IActionResult OnPostAllow(int id)
        {
            Models.Comment com = _comments.GetCommentById(id);
            com.IsAllow = true;

            _db.Comment.Update(com);
            _db.SaveChanges();

            return RedirectToPage("CommentsList");
        }

        public IActionResult OnPostDelete(int id)
        {
            Models.Comment com = _comments.GetCommentById(id);

            _db.Comment.Remove(com);
            _db.SaveChanges();

            return RedirectToPage("CommentsList");
        }
    }
}
