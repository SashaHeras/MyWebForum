using MyWebForum.Data.Infrastructure;
using MyWebForum.Models;
using System.Linq;

namespace MyWebForum.Data.Interfaces
{
    public interface ICommentRepository : ITransientService, IRepository<Comment>
    {
        public Comment GetCommentById(int id);

        public IQueryable<Comment> GetCommentsByPostId(int id);

        public IEnumerable<Comment> GetAllAllowedComments(int id);
    }
}
