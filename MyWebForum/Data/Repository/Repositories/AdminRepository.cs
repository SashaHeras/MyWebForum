﻿using MyWebForum.Data.Interfaces;
using MyWebForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebForum.Data.Repository.Repositories
{
    public class AdminRepository : Repository<User>, IAdminRepository
    {
        public AdminRepository(MyForumContext forumContext) : base(forumContext)
        {

        }

        public User GetAdminById(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);    
        }
    }
}
