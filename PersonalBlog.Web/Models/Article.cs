using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.Web.Models
{
    public class Article
    {
        public Article()
        {
            
        }

        public Article(string title, string content)
        {
            Title = title;
            PublishedAt = DateTime.Now;
            Content = content;
        }

        public int Id { get; set; }
        public bool IsNew => Id == 0;
        public string Title { get; set; }

        public DateTime PublishedAt { get; set; }
        public string Content { get; set; }
    }
}
