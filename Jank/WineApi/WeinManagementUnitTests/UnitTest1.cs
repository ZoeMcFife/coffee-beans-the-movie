using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WineApi.Context;
using WineApi.Controllers;
using WineApi.Models;
using WineApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Castle.Core.Configuration;
using System.Linq;

namespace WineApi.Tests
{
    [TestClass]
    public class UsersControllerTests
    {
        private Mock<WineDbContext> _mockContext;
        private Mock<IConfiguration> _mockConfig;
        private UsersController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockContext = new Mock<WineDbContext>();
            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.Setup(c => c["Jwt:Key"]).Returns("test_key");
            _mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("test_issuer");
            _mockConfig.Setup(c => c["Jwt:Audience"]).Returns("test_audience");

            _controller = new UsersController(_mockContext.Object, _mockConfig.Object);
        }

        [TestMethod]
        public async Task Register_User_Returns_CreatedAtActionResult()
        {
            // Arrange
            var userDto = new UserRegisterDTO
            {
                Username = "testuser",
                Password = "password123",
                Email = "testuser@example.com"
            };

            var mockDbSet = new Mock<DbSet<User>>();
            _mockContext.Setup(m => m.Users).Returns(mockDbSet.Object);

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public async Task Register_User_UsernameTaken_Returns_BadRequest()
        {
            // Arrange
            var userDto = new UserRegisterDTO
            {
                Username = "existinguser",
                Password = "password123",
                Email = "existinguser@example.com"
            };

            var mockDbSet = new Mock<DbSet<User>>();
            var users = new List<User> { new User { Username = "existinguser" } };
            var queryable = users.AsQueryable();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            _mockContext.Setup(m => m.Users).Returns(mockDbSet.Object);

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Login_User_Returns_OkResult()
        {
            // Arrange
            var userLogin = new UserLoginDTO
            {
                Email = "testuser@example.com",
                Password = "password123"
            };

            var mockDbSet = new Mock<DbSet<User>>();
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "testuser",
                    Email = "testuser@example.com",
                    Password = _controller.HashPassword("password123")
                }
            };

            var queryable = users.AsQueryable();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            _mockContext.Setup(m => m.Users).Returns(mockDbSet.Object);

            // Act
            var result = await _controller.Login(userLogin);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetUser_NoClaim_Returns_Unauthorized()
        {
            // Arrange
            var mockUser = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { }));
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = mockUser };

            // Act
            var result = await _controller.GetUser();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public async Task DeleteUser_ValidUserId_Returns_NoContent()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim("userId", "1")
            };
            var mockUser = new ClaimsPrincipal(new ClaimsIdentity(claims));
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = mockUser };

            var user = new User { Id = 1, Username = "testuser" };

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.Setup(m => m.FindAsync(1)).ReturnsAsync(user);

            _mockContext.Setup(m => m.Users).Returns(mockDbSet.Object);

            // Act
            var result = await _controller.DeleteUser();

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}
