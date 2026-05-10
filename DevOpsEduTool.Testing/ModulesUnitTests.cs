using DevOpsEduTool.Controllers;
using DevOpsEduTool.Data;
using DevOpsEduTool.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;
using NUnit.Framework;


namespace DevOpsEduTool.Testing
{
    [TestFixture]
    public class ModulesUnitTests
    {
        private DevOpsEduContext _db;
        private Mock<IAuthenticationService> _authServiceMock;
        private ModulesController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DevOpsEduContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _db = new DevOpsEduContext(options);
            _db.Database.EnsureCreated();

            _authServiceMock = new Mock<IAuthenticationService>();

            var urlHelperMock = new Mock<IUrlHelper>();

            var tempDataMock = new Mock<ITempDataDictionary>();

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(s => s.GetService(typeof(IAuthenticationService)))
                .Returns(_authServiceMock.Object);

            var tempDataFactoryMock = new Mock<ITempDataDictionaryFactory>();
            serviceProviderMock
                .Setup(s => s.GetService(typeof(ITempDataDictionaryFactory)))
                .Returns(tempDataFactoryMock.Object);

            _controller = new ModulesController(_db);

            var claims = new List<Claim>
            {
                new Claim("UserId", "99"),
                new Claim(ClaimTypes.Name, "TestStudent")
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }


        [TearDown]
        public void TearDown()
        {
            _db.Dispose();
            _controller.Dispose();
        }


        [Test]
        public async Task DevOpsBasics_ReturnsViewWithModuleOne()
        {
            // Act
            var result = await _controller.DevOpsBasics() as ViewResult;
            var model = result?.Model as Module;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(model?.Id, Is.EqualTo(1));
            Assert.That(model?.Name, Is.EqualTo("DevOps Fundamentals"));
        }

        [Test]
        public async Task NewProgress_SavesToDatabase()
        {
            // Act
            var result = await _controller.SaveProgress(1, "completed");

            // Assert
            var progress = await _db.UserProgress.FirstOrDefaultAsync(p => p.UserId == 99 && p.ModuleId == 1);
            Assert.That(progress, Is.Not.Null);
            Assert.That(progress.Status, Is.EqualTo("completed"));
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        [Test]
        public async Task UpdateExistingProgress_Successful()
        {
            // Arrange
            _db.UserProgress.Add(new UserProgress { UserId = 99, ModuleId = 1, Status = "started" });
            await _db.SaveChangesAsync();

            // Act
            await _controller.SaveProgress(1, "completed");

            // Assert
            var progress = await _db.UserProgress.FirstAsync(p => p.UserId == 99 && p.ModuleId == 1);
            Assert.That(progress.Status, Is.EqualTo("completed"));
        }

        [Test]
        public async Task GetUserProgress_ReturnsCorrectJson()
        {
            // Arrange
            _db.UserProgress.Add(new UserProgress { UserId = 99, ModuleId = 1, Status = "completed" });
            await _db.SaveChangesAsync();

            // Act
            var result = await _controller.GetUserProgress() as JsonResult;

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task ClearProgress_RemovesAllEntriesForUser()
        {
            // Arrange
            _db.UserProgress.Add(new UserProgress { UserId = 99, ModuleId = 1, Status = "completed" });
            _db.UserProgress.Add(new UserProgress { UserId = 99, ModuleId = 2, Status = "completed" });
            _db.UserProgress.Add(new UserProgress { UserId = 1, ModuleId = 1, Status = "completed" }); // Different user
            await _db.SaveChangesAsync();

            // Act
            await _controller.ClearProgress();

            // Assert
            var user99Progress = _db.UserProgress.Where(p => p.UserId == 99).Count();
            var user1Progress = _db.UserProgress.Where(p => p.UserId == 1).Count();

            Assert.That(user99Progress, Is.EqualTo(0));
            Assert.That(user1Progress, Is.EqualTo(1));
        }

        [Test]
        public async Task SaveProgress_NoUser_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // Act
            var result = await _controller.SaveProgress(1);

            // Assert
            Assert.That(result, Is.InstanceOf<UnauthorizedResult>());
        }

    }
}
