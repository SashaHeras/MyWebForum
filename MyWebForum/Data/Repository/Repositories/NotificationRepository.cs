using MyWebForum.Data.Interfaces;
using MyWebForum.Models;

namespace MyWebForum.Data.Repository.Repositories
{
    public class NotificationRepository : Repository<Notification>, INotificationInterface
    {
        public NotificationRepository(MyForumContext forumContext) : base(forumContext)
        {

        }

        public Notification GetNotificationById(int id)
        {
            return GetAll().Where(n => n.Id == id).FirstOrDefault();
        }

        public IEnumerable<Notification> GetNotificationsByUserId(int id)
        {
            return GetAll().Where(n => n.UserId == id);
        }

        public int GetUsersUnreadedNotificationCount(int id)
        {
            return GetAll().Where(n => n.UserId == id && n.Checked == false).Count();
        }
    }
}
