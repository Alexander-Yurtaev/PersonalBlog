using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PersonalBlog.Web.Controllers;
using PersonalBlog.Web.Models;
using PersonalBlog.Web.Repositories;

namespace PersonalBlog.Tests
{
    public class AdminControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewResult_WithArticles()
        {
            // Arrange
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.GetAll())
                .ReturnsAsync(new List<Article>());

            var controller = new AdminController(mockRepo.Object, Mock.Of<ILogger<HomeController>>());

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<IEnumerable<Article>>(viewResult.Model);
            mockRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void ShowAddArticle_ReturnsViewResult()
        {
            var controller = new AdminController(Mock.Of<IRepository>(), Mock.Of<ILogger<HomeController>>());
            var result = controller.ShowAddArticle();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task CreateOrUpdateArticle_NewArticle_CallsCreate()
        {
            // Arrange
            var article = new Article();
            var mockRepo = new Mock<IRepository>();
            var controller = new AdminController(mockRepo.Object, Mock.Of<ILogger<HomeController>>());

            // Act
            await controller.CreateOrUpdateArticle(article);

            // Assert
            mockRepo.Verify(r => r.Create(article), Times.Once);
            mockRepo.Verify(r => r.Update(It.IsAny<int>(), article), Times.Never);
        }

        [Fact]
        public async Task CreateOrUpdateArticle_ExistingArticle_CallsUpdate()
        {
            // Arrange
            var article = new Article { Id = 1 };
            var mockRepo = new Mock<IRepository>();
            var controller = new AdminController(mockRepo.Object, Mock.Of<ILogger<HomeController>>());

            // Act
            await controller.CreateOrUpdateArticle(article);

            // Assert
            mockRepo.Verify(r => r.Update(article.Id, article), Times.Once);
            mockRepo.Verify(r => r.Create(article), Times.Never);
        }

        [Fact]
        public async Task ShowUpdateArticle_ArticleExists_ReturnsView()
        {
            // Arrange
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Article());

            var controller = new AdminController(mockRepo.Object, Mock.Of<ILogger<HomeController>>());

            // Act
            var result = await controller.ShowUpdateArticle(1);

            // Assert
            Assert.IsType<ViewResult>(result);
            mockRepo.Verify(r => r.GetById(1), Times.Once);
        }

        [Fact]
        public async Task ShowUpdateArticle_ArticleNotFound_ReturnsNotFound()
        {
            // Arrange
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync((Article)null);

            var controller = new AdminController(mockRepo.Object, Mock.Of<ILogger<HomeController>>());

            // Act
            var result = await controller.ShowUpdateArticle(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_CallsRepositoryDelete()
        {
            // Arrange
            var mockRepo = new Mock<IRepository>();
            var controller = new AdminController(mockRepo.Object, Mock.Of<ILogger<HomeController>>());

            // Act
            await controller.Delete(1);

            // Assert
            mockRepo.Verify(r => r.Delete(1), Times.Once);
        }
    }
}
