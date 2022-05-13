using MyWebForum.Data.Infrastructure;
using MyWebForum.Models;

namespace MyWebForum.Data.Interfaces
{
    public interface INotificationInterface : ISingletonService, IRepository<Notification>
    {
        public Notification GetNotificationById(int id);

        public IEnumerable<Notification> GetNotificationsByUserId(int id);

        public int GetUsersUnreadedNotificationCount(int id);
    }
}
