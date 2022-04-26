using MyWebForum.Data.Infrastructure;
using MyWebForum.Models;

namespace MyWebForum.Data.Interfaces
{
    public interface IUserRepository : ISingletonService, IRepository<User>
    {
        public User GetUserById(int id);

        public string GetUserName(User u);

        public User GetUser(string email, string password);

        public string GetUserNameById(int id);
    }
}
