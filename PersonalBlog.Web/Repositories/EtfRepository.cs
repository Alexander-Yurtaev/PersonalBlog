using Microsoft.EntityFrameworkCore;
using PersonalBlog.Web.Data;
using PersonalBlog.Web.Models;

namespace PersonalBlog.Web.Repositories
{
    public class EtfRepository : IRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EtfRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Article>> GetAll()
        {
            return await _dbContext.Articles.ToListAsync();
        }

        public async Task<Article?> GetById(int id)
        {
            var article = await _dbContext.FindAsync<Article>(id);
            return article;
        }

        public async Task<Article> Create(Article article)
        {
            ArgumentNullException.ThrowIfNull(article);

            if (article is { Id: 0, PublishedAt: null })
            {
                article.PublishedAt = DateTime.UtcNow;
            }

            _dbContext.Add(article);
            await _dbContext.SaveChangesAsync();
            return article!;
        }

        public async Task<Article> Update(int id, Article article)
        {
            var existingArticle = await GetById(id);
            if (existingArticle == null)
                throw new InvalidOperationException("Article not found");

            _dbContext.Articles.Update(article);
            await _dbContext.SaveChangesAsync();
            return article;
        }

        public async Task Delete(int id)
        {
            var article = await GetById(id);
            if (article is null)
            {
                return;
            }

            _dbContext.Articles.Remove(article);
            await _dbContext.SaveChangesAsync();
        }
    }
}
