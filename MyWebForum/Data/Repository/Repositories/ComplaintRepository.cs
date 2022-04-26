using MyWebForum.Data.Interfaces;
using MyWebForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebForum.Data.Repository.Repositories
{
    public class ComplaintRepository : Repository<Complaint>, IComplaintRepository
    {
        public ComplaintRepository(MyForumContext repositoryContext) : base(repositoryContext)
        {

        }

        public Complaint GetComplaintById(int id)
        {
            return GetAll().Where(c => c.Id == id).FirstOrDefault();
        }
    }
}
