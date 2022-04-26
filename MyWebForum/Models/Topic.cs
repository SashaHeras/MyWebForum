using System.ComponentModel.DataAnnotations;

namespace MyWebForum.Models
{
    public class Topic
    {
        public int TopicId { get; set; }
        
        [Display(Name = "TopicName")]
        public string TopicName { get; set; }

        public bool IsAllow { get; set; }
    }
}
