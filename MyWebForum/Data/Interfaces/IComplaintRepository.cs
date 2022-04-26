using MyWebForum.Data.Infrastructure;
using MyWebForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebForum.Data.Interfaces
{
    public interface IComplaintRepository: ITransientService, IRepository<Complaint>
    {
        public Complaint GetComplaintById(int id);
    }
}
