using PersonalBlog.Web.Models;

namespace PersonalBlog.Web.Repositories;

public interface IRepository
{
    Task<List<Article>> GetAll();
    Task<Article?> GetById(int id);
    Task<Article> Create(Article article);
    Task<Article> Update(int id, Article article);
    Task Delete(int id);
}