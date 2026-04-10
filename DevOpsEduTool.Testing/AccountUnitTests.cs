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

namespace DevOpsEduTool.Testing
{
    [TestFixture]
    public class AccountControllerTests
    {
        private DevOpsEduContext _db;
        private Mock<IAuthenticationService> _authServiceMock;
        private AccountController _controller;

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

            _controller = new AccountController(_db)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { RequestServices = serviceProviderMock.Object }
                },
                Url = urlHelperMock.Object,
                TempData = tempDataMock.Object
            };
        }

        [TearDown]
        public void TearDown()
        {
            _db.Dispose();
            _controller.Dispose();
        }

        [Test]
        public async Task Login_WithCorrectCredentials_RedirectsToIndex()
        {
            // Arrange
            var user = new User { Username = "devops_student", PasswordHash = "password123", Email = "test@edu.com" };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            // Act
            var result = await _controller.Login("devops_student", "password123");

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));

            _authServiceMock.Verify(s => s.SignInAsync(
                It.IsAny<HttpContext>(),
                "Cookies",
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<AuthenticationProperties>()),
            Times.Once);
        }

        [Test]
        public async Task Login_WithWrongCredentials_DoesNotSignIn()
        {
            // Arrange
            var user = new User { Username = "user1", Email = "user@mail.com",PasswordHash = "correct_pass" };
            _db.Users.Add(user);
            _db.SaveChanges();

            // Act
            await _controller.Login("user1", "wrong_pass");

            // Assert
            _authServiceMock.Verify(s => s.SignInAsync(
                It.IsAny<HttpContext>(),
                It.IsAny<string>(),
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<AuthenticationProperties>()),
            Times.Never);
        }

        [Test]
        public void Register_NewUser_AddsToDatabase()
        {
            // Arrange
            var newUser = new User { Username = "tester", Email = "test@test.com" };

            // Act
            var result = _controller.Register(newUser, "mypassword");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_db.Users.Count(), Is.EqualTo(1));
                Assert.That(_db.Users.First().Username, Is.EqualTo("tester"));
                Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            });
        }

        [Test]
        public void Register_DuplicateEmail_ReturnsViewWithErrorMessage()
        {
            // Arrange
            _db.Users.Add(new User { Username = "original", Email = "duplicate@mail.com", PasswordHash = "123" });
            _db.SaveChanges();

            var duplicateUser = new User { Username = "new", Email = "duplicate@mail.com" };

            // Act
            var result = _controller.Register(duplicateUser, "pass");

            // Assert
            var viewResult = result as ViewResult;
            Assert.That(_controller.ModelState.IsValid, Is.False);
            Assert.That(viewResult.Model, Is.EqualTo(duplicateUser));
        }
    }
}