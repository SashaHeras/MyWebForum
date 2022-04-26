using MyWebForum.Data.Interfaces;
using MyWebForum.Models;
using System.Linq;

namespace MyWebForum.Data.Repository.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(MyForumContext forumContext) : base(forumContext)
        {

        }

        public IEnumerable<Comment> GetAllAllowedComments(int id)
        {
            return GetAll().Where(c => c.PostId == id && c.IsAllow == true);
        }

        public Comment GetCommentById(int id)
        {
            return GetAll().Where(c => c.CommentId == id).FirstOrDefault();
        }

        public IQueryable<Comment> GetCommentsByPostId(int id)
        {
            return GetAll().Where(c => c.PostId == id);
        }
    }
}
