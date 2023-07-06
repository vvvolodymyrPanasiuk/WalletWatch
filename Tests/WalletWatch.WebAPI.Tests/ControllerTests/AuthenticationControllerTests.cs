using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WalletWatch.Domain.Entities.UserAggregate;
using WalletWatch.WebAPI.Controllers;
using WalletWatch.WebAPI.Models.AuthenticationModels.Request;

namespace WalletWatch.WebAPI.Tests.ControllerTests
{
    [TestFixture]
    public class AuthenticationControllerTests
    {
        private AuthenticationController _accountController;
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<SignInManager<User>> _mockSignInManager;
        private Mock<ILogger<AuthenticationController>> _mockLogger;

        [SetUp]
        public void Setup()
        {
            // Mock UserManager
            _mockUserManager = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

            // Mock SignInManager
            _mockSignInManager = new Mock<SignInManager<User>>(
                _mockUserManager.Object, Mock.Of<IHttpContextAccessor>(), 
                Mock.Of<IUserClaimsPrincipalFactory<User>>(), null, null, null, null);

            // Mock ILogger
            _mockLogger = new Mock<ILogger<AuthenticationController>>();

            // Create instance of AccountController with mocked dependencies
            _accountController = new AuthenticationController(
                _mockUserManager.Object, _mockSignInManager.Object, _mockLogger.Object);
        }

        [Test]
        public async Task Register_ValidModel_ReturnsCreated()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@example.com",
                Password = "P@ssw0rd"
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(registerRequest.Email)).ReturnsAsync((User)null);
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), registerRequest.Password)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _accountController.Register(registerRequest);

            // Assert
            Assert.That(result, Is.TypeOf<StatusCodeResult>());
            var statusCodeResult = (StatusCodeResult)result;
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        }

        [Test]
        public async Task Register_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "", // Invalid email
                Password = "P@ssw0rd"
            };

            _accountController.ModelState.AddModelError("Email", "Email is required");

            // Act
            var result = await _accountController.Register(registerRequest);

            // Assert
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        }

        [Test]
        public async Task Login_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "", // Invalid email
                Password = "P@ssw0rd"
            };

            _accountController.ModelState.AddModelError("Email", "Email is required");

            // Act
            var result = await _accountController.Login(loginRequest);

            // Assert
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        }
    }
}

