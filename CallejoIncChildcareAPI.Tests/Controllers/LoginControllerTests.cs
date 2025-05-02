using CallejoIncChildCareAPI.Authentication;
using CallejoIncChildCareAPI.Controllers;
using Common.Services.Login;
using Common.View;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CallejoIncChildCareAPI.Tests
{
    public class LoginControllerTests
    {
        private readonly Mock<ILoginService> _mockLoginService;
        private readonly LoginController _controller;

        public LoginControllerTests()
        {
            _mockLoginService = new Mock<ILoginService>();
            _controller = new LoginController(_mockLoginService.Object);
        }

        [Fact]
        public void Login_ReturnsBadRequest_WhenLoginInfoIsNull()
        {
            // Act
            var result = _controller.Login(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var apiResponse = Assert.IsType<APIResponse>(badRequestResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal("Username and password are required.", apiResponse.Message);
        }

        [Fact]
        public void Login_ReturnsBadRequest_WhenUserNameOrPasswordIsEmpty()
        {
            // Arrange
            var loginInfo = new LoginView { UserName = "", Password = "" };

            // Act
            var result = _controller.Login(loginInfo);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var apiResponse = Assert.IsType<APIResponse>(badRequestResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal("Username and password are required.", apiResponse.Message);
        }

        [Fact]
        public void Login_ReturnsOkResult_WhenLoginIsSuccessful()
        {
            // Arrange
            var loginInfo = new LoginView { UserName = "testuser", Password = "testpassword" };
            var apiResponse = new APIResponse { Success = true };
            _mockLoginService.Setup(s => s.LoginUser(loginInfo.UserName, loginInfo.Password, AuthenticateAction.Key)).Returns(apiResponse);

            // Act
            var result = _controller.Login(loginInfo);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(apiResponse, okResult.Value);
        }

        [Fact]
        public void Login_ReturnsUnauthorized_WhenLoginFails()
        {
            // Arrange
            var loginInfo = new LoginView { UserName = "testuser", Password = "testpassword" };
            var apiResponse = new APIResponse { Success = false };
            _mockLoginService.Setup(s => s.LoginUser(loginInfo.UserName, loginInfo.Password, AuthenticateAction.Key)).Returns(apiResponse);

            // Act
            var result = _controller.Login(loginInfo);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
            Assert.Equal(apiResponse, unauthorizedResult.Value);
        }

        [Fact]
        public void IsAuthenticated_ReturnsBadRequest_WhenLoginInfoIsNull()
        {
            // Act
            var result = _controller.IsAuthenticated(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.False((bool)badRequestResult.Value);
        }

        [Fact]
        public void IsAuthenticated_ReturnsBadRequest_WhenUserNameOrCookieIsInvalid()
        {
            // Arrange
            var loginInfo = new LoginView { UserName = "", AuthenticationCookie = Guid.Empty };

            // Act
            var result = _controller.IsAuthenticated(loginInfo);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.False((bool)badRequestResult.Value);
        }

        [Fact]
        public void IsAuthenticated_ReturnsOkResult_WhenUserIsAuthenticated()
        {
            // Arrange
            var loginInfo = new LoginView { UserName = "testuser", AuthenticationCookie = Guid.NewGuid() };
            _mockLoginService.Setup(s => s.IsUserAuthenticated(loginInfo.UserName, loginInfo.AuthenticationCookie.Value)).Returns(true);

            // Act
            var result = _controller.IsAuthenticated(loginInfo);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public void IsAuthenticated_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var loginInfo = new LoginView { UserName = "testuser", AuthenticationCookie = Guid.NewGuid() };
            _mockLoginService.Setup(s => s.IsUserAuthenticated(loginInfo.UserName, loginInfo.AuthenticationCookie.Value)).Returns(false);

            // Act
            var result = _controller.IsAuthenticated(loginInfo);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
            Assert.False((bool)unauthorizedResult.Value);
        }
    }
}
