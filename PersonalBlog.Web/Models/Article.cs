namespace PersonalBlog.Web.Models
{
    public class Article
    {
        public Article()
        {
            PublishedAt = DateTime.Now;
        }

        public Article(string title, string content) : this()
        {
            Title = title;
            Content = content;
        }

        public int Id { get; set; }
        public bool IsNew => Id == 0;
        public string Title { get; set; }

        public DateTime? PublishedAt { get; set; }
        public string Content { get; set; }
    }
}
