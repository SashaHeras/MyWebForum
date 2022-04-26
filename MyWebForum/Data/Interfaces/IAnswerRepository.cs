using MyWebForum.Data.Infrastructure;
using MyWebForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebForum.Data.Interfaces
{
    public interface IAnswerRepository : ITransientService, IRepository<UserPollAnswer>
    {
        public IQueryable<UserPollAnswer> GetAll();

        public UserPollAnswer GetById(int id);

        public int CountAnswersOnQuestion(int id);

        public UserPollAnswer GetByUserIdAndPollId(int uid, int pid);

        public IQueryable<UserPollAnswer> GetByQuestionId(int id);
    }
}
