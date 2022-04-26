using MyWebForum.Data.Infrastructure;
using MyWebForum.Models;
using System.Linq;

namespace MyWebForum.Data.Interfaces
{
    public interface ITopicRepository : ITransientService, IRepository<Topic>
    {
        public Topic GetTopicById(int id);

        public IQueryable<Topic> GetTopicByName(string name);

        public IQueryable<Topic> GetAllowedTopics();

        public IQueryable<Topic> SearchAllowedTopics(string topic);
    }
}
