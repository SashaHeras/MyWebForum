using MyWebForum.Data.Infrastructure;
using MyWebForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebForum.Data.Interfaces
{
    public interface IQuestionRepository : ITransientService, IRepository<PollQuestion>
    {
        public IQueryable<PollQuestion> GetAll();

        public PollQuestion GetQuestionById(int id);

        public IQueryable<PollQuestion> GetByPollId(int pollId);
    }
}
