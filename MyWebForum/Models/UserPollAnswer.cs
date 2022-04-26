using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebForum.Models
{
    public class UserPollAnswer
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public int UserId { get; set; }
    }
}
