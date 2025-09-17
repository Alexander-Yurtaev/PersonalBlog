using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Web.Models;
using PersonalBlog.Web.Repositories;

namespace PersonalBlog.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IRepository _repository;
        private readonly ILogger<HomeController> _logger;

        public AdminController(IRepository repository, ILogger<HomeController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _repository.GetAll();
            return View(articles);
        }

        public IActionResult AddArticle()
        {
            return View();
        }

        public async Task<IActionResult> SaveArticle(Article article)
        {
            if (article.IsNew)
            {
                await _repository.Create(article);
            }
            else
            {
                await _repository.Update(article.Id, article);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateArticle(int id)
        {
            var article = await _repository.GetById(id);
            if (article == null)
            {
                return NotFound(id);
            }

            return View(article);
        }

        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            var article = await _repository.GetById(id);
            if (article is null)
            {
                return NotFound();
            }

            return View(article);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _repository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
