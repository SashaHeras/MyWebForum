namespace MyWebForum.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }

        public DateTime Date { get; set; }

        public bool Checked { get; set; }
    }
}
