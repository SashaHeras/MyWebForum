﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebForum.Models
{
    public class Poll
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string Description { get; set; }

        public int CountViews { get; set; }

        public int CountQuestions { get; set; }

        public int UserId { get; set; }

        public bool IsAllowed { get; set; }
    }
}
