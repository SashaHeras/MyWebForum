using MyWebForum.Data;
using MyWebForum.Data.Interfaces;
using MyWebForum.Data.Repository.Repositories;
using MyWebForum.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyWebForum.Data.Repository.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MyForumContext forumContext) : base(forumContext)
        {

        }

        public User GetUserById(int id)
        {
            return GetAll().Where(u=>u.Id == id).FirstOrDefault();
        }

        public string GetUserName(User u)
        {
            return u.Name;
        }

        public string GetUserNameById(int id)
        {
            return GetAll().Where(u => u.Id == id).FirstOrDefault().Name.ToString();
        }

        public User GetUserNameByEmail(string email)
        {
            return GetAll().Where(u => u.Email.CompareTo(email) == 0).FirstOrDefault();
        }

        public IEnumerable<User> GetAllWithoutUser(int id)
        {
            return GetAll().Except(GetAll().Where(u => u.Id == id));  
        }

        public User GetUser(string email, string password)
        {
            return GetAll().Where(u => u.Email.CompareTo(email) == 0 && u.Password.CompareTo(password) == 0).FirstOrDefault();
        }
    }
}
