using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PersonalBlog.Web.Controllers;
using PersonalBlog.Web.Models;
using PersonalBlog.Web.Repositories;

namespace PersonalBlog.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewResult_WithArticles()
        {
            // Arrange
            var mockRepo = new Mock<IRepository>();
            var articles = new List<Article> { new Article { Id = 1, Title = "Test" } };
            mockRepo.Setup(r => r.GetAll())
                .ReturnsAsync(articles);

            var controller = new HomeController(mockRepo.Object, Mock.Of<ILogger<HomeController>>());

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<IEnumerable<Article>>(viewResult.Model);
            mockRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public async Task Article_ExistingId_ReturnsView()
        {
            // Arrange
            var mockRepo = new Mock<IRepository>();
            var article = new Article { Id = 1, Title = "Test" };
            mockRepo.Setup(r => r.GetById(1))
                .ReturnsAsync(article);

            var controller = new HomeController(mockRepo.Object, Mock.Of<ILogger<HomeController>>());

            // Act
            var result = await controller.Article(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(article, viewResult.Model);
            mockRepo.Verify(r => r.GetById(1), Times.Once);
        }

        [Fact]
        public async Task Article_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync((Article)null);

            var controller = new HomeController(mockRepo.Object, Mock.Of<ILogger<HomeController>>());

            // Act
            var result = await controller.Article(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            mockRepo.Verify(r => r.GetById(1), Times.Once);
        }

        [Fact]
        public async Task Article_Exception_ReturnsStatusCode500()
        {
            // Arrange
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.GetById(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            var controller = new HomeController(mockRepo.Object, Mock.Of<ILogger<HomeController>>());

            // Act
            var result = await controller.Article(1);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Privacy_ReturnsViewResult()
        {
            // Arrange
            var controller = new HomeController(Mock.Of<IRepository>(), Mock.Of<ILogger<HomeController>>());

            // Act
            var result = controller.Privacy();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Error_ReturnsViewResult_WithErrorViewModel()
        {
            // Arrange
            // Создаем мок HttpContext
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.TraceIdentifier).Returns("test-trace-id");

            var controller = new HomeController(Mock.Of<IRepository>(), Mock.Of<ILogger<HomeController>>());
            
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            // Act
            var result = controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
            Assert.NotNull(model.RequestId);
        }
    }
}
