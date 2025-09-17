using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Web.Models;
using PersonalBlog.Web.Repositories;

namespace PersonalBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IRepository repository, ILogger<HomeController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _repository.GetAll();
            return View(articles);
        }

        public async Task<IActionResult> Article(int id)
        {
            try
            {
                var article = await _repository.GetById(id);

                // Добавьте проверку на null
                if (article == null)
                {
                    return NotFound();
                }

                return View(article);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении статьи");
                return StatusCode(500, "Произошла ошибка при загрузке статьи");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
