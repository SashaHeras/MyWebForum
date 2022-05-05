using MyWebForum.Data.Interfaces;
using MyWebForum.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyWebForum.Data.Repository.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(MyForumContext forumContext) : base(forumContext)
        {

        }

        public IQueryable<Post> GetAllowedPostsByTopicId(int id)
        {
            return GetAll().Where(p => p.TopicId == id && p.IsAllow == true);
        }

        public IEnumerable<Post> GetPopularAllowedPosts(int n)
        {
            return GetAll().Where(p => p.IsAllow == true).OrderByDescending(p => p.Views).Take(n);
        }

        public Post GetPostById(int id)
        {
            return GetAll().Where(p => p.PostId == id).FirstOrDefault();
        }

        public Post GetPostByTopicId(int id)
        {
            return GetAll().Where(p => p.TopicId == id).FirstOrDefault();
        }

        public IQueryable<Post> GetPostsByTopicId(int id)
        {
            return GetAll().Where(p => p.TopicId == id);
        }

        public IQueryable<Post> GetPostsByUserId(int id)
        {
            return GetAll().Where(p => p.UserId == id);
        }


    }
}
