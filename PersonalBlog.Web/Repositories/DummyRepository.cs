using PersonalBlog.Web.Models;

namespace PersonalBlog.Web.Repositories
{
    public class DummyRepository : IRepository
    {
        private List<Article> _testArticles = new List<Article>();

        public Task<List<Article>> GetAll()
        {
            return Task.FromResult(GetCachedArticles());
        }

        public Task<Article?> GetById(int id)
        {
            var result = GetCachedArticles().FirstOrDefault(a => a.Id == id);
            return Task.FromResult(result);
        }

        public Task<Article> Create(Article article)
        {
            article.Id = GetNextId();
            article.PublishedAt = DateTime.Now;
            _testArticles.Add(article);

            return Task.FromResult(article);
        }

        public Task<Article> Update(int id, Article source)
        {
            ArgumentNullException.ThrowIfNull(source);

            var dest= _testArticles.FirstOrDefault(a => a.Id == id);
            
            if (dest is null) throw new KeyNotFoundException();

            dest.Title = source.Title;
            dest.Content = source.Content;

            return Task.FromResult(dest);
        }

        public Task Delete(int id)
        {
            var article = _testArticles.FirstOrDefault(a => a.Id == id);
            if (article is not null)
            {
                _testArticles.Remove(article);
            }

            return Task.CompletedTask;
        }

        private List<Article> GetCachedArticles()
        {
            if (!_testArticles.Any())
            {
                _testArticles = GetAllArticles();
            }

            return _testArticles;
        }

        private int GetNextId()
        {
            if (!_testArticles.Any())
            {
                return 1;
            }

            return _testArticles.Max(a => a.Id) + 1;
        }

        private List<Article> GetAllArticles()
        {
            var testArticles = new List<Article>
            {
                new Article(title: "Введение в программирование", content: "Эта статья представляет собой введение в основы программирования...")
                {
                    Id = 1,
                    PublishedAt = new DateTime(2024, 8, 7)
                },

                new Article(title: "Основы работы с базами данных", content: "В этой статье мы рассмотрим основные концепции работы с БД...")
                {
                    Id = 2,
                    PublishedAt = new DateTime(2024, 7, 15)
                },

                new Article(title: "Практическое руководство по ASP.NET Core", content: "Пошаговая инструкция по созданию веб-приложений на ASP.NET Core...")
                {
                    Id = 3,
                    
                    PublishedAt = new DateTime(2024, 6, 22)
                },

                new Article(title: "Оптимизация производительности", content: "Методы улучшения производительности веб-приложений...")
                {
                    Id = 4,
                    PublishedAt = new DateTime(2024, 5, 10)
                },

                new Article(title: "Тестирование программного обеспечения", content: "Полное руководство по тестированию ПО...")
                {
                    Id = 5,
                    PublishedAt = new DateTime(2024, 4, 25)
                }
            };

            return testArticles;
        }
    }
}
