using MyWebForum.Data.Infrastructure;
using MyWebForum.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyWebForum.Data.Interfaces
{
    public interface IPostRepository :ITransientService, IRepository<Post>
    {
        public Post GetPostById(int id);

        public Post GetPostByTopicId(int id);

        public IQueryable<Post> GetPostsByTopicId(int id);

        public IQueryable<Post> GetPostsByUserId(int id);

        public IQueryable<Post> GetAllowedPostsByTopicId(int id);

        public IEnumerable<Post> GetPopularAllowedPosts(int n);
    }
}