using MyWebForum.Data.Infrastructure;
using MyWebForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebForum.Data.Interfaces
{
    public interface IAdminRepository : ISingletonService, IRepository<User>
    {
        public User GetAdminById(int id);
    }
}
