using MyWebForum.Data.Interfaces;
using MyWebForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebForum.Data.Repository.Repositories
{
    public class MarkRepository : Repository<UserPostMark>, IMarkRepository
    {
        private MyForumContext _db;

        public MarkRepository(MyForumContext forumContext) : base(forumContext)
        {
            _db = forumContext;
        }

        public UserPostMark GetMarkById(int id)
        {
            return GetAll().Where(m => m.Id == id).FirstOrDefault();
        }

        public IQueryable<UserPostMark> GetMarksByPostId(int id)
        {
            return GetAll().Where(m => m.PostId == id);
        }

        public UserPostMark GetMarkByUserAndPostId(int ui, int pi)
        {
            return GetAll().Where(m => m.UserId == ui && m.PostId == pi).FirstOrDefault();
        }

        public int GetPostMark(int id)
        {
            return GetAll().Where(p => p.PostId == id).Sum(m => m.PostMark);
        }

        public UserPostMark GetUsersMarkById(int id)
        {
            return GetAll().Where(m => m.UserId == id).FirstOrDefault();
        }

        public double GetAverageUserPostMark(int id)
        {
            double avg = 0;

            IEnumerable<Models.Post> posts = _db.Post.Where(p => p.UserId == id);

            foreach(var post in posts)
            {
                avg += _db.Mark.Where(m => m.PostId == post.PostId).Sum(p => p.PostMark);
            }

            return Math.Round((avg / posts.Count()), 2);
        }
    }
}
