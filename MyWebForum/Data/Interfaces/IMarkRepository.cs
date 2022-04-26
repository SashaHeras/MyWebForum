using MyWebForum.Data.Infrastructure;
using MyWebForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebForum.Data.Interfaces
{
    public interface IMarkRepository : ITransientService, IRepository<UserPostMark>
    {
        public UserPostMark GetMarkById(int id);

        public UserPostMark GetUsersMarkById(int id);

        public IQueryable<UserPostMark> GetMarksByPostId(int id);

        public int GetPostMark(int id);

        public UserPostMark GetMarkByUserAndPostId(int ui, int pi);
    }
}
