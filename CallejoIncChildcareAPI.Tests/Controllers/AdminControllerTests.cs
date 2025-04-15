using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CallejoIncChildcareAPI.Controllers;
using Common.Models.Data;
using Common.Services.Login;
using Common.Services.User;
using Common.View;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace CallejoIncChildcareAPI.Tests
{
    public class FakeImageService : ImageService
    {
        public FakeImageService()
            : base(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    { "ConnectionStrings:DataContext", "Server=localhost;Database=fake;Trusted_Connection=True;" }
                })
                .Build())
        { }

        public new Task SaveImageUrlAsync(string imageUrl) => Task.CompletedTask;

        public new Task<string> GetLatestImageUrlAsync() => Task.FromResult("https://test.com/fake.jpg");
    }

    public class AdminControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly FakeImageService _fakeImageService;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly AdminController _controller;
        private Mock<ILoginService> _loginService;

        public AdminControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _fakeImageService = new FakeImageService();
            _mockConfig = new Mock<IConfiguration>();
            _loginService = new Mock<ILoginService>();

            _controller = new AdminController(
                _mockUserService.Object,
                _fakeImageService,
                _mockConfig.Object,
                null, // DbContext can be mocked in integration tests
                _loginService.Object
            );
        }

        [Fact]
        public void InsertUser_ReturnsOk_WhenSuccessful()
        {
            var userDto = new AdminUserCreationDTO { Email = "test@email.com", Password = "Password123" };
            var expected = new APIResponse { Success = true };
            _mockUserService.Setup(x => x.InsertUser(It.IsAny<AdminUserCreationDTO>())).Returns(expected);

            var result = _controller.InsertUser(userDto);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var api = Assert.IsType<APIResponse>(okResult.Value);
            Assert.True(api.Success);
        }

        [Fact]
        public void InsertUser_ReturnsBadRequest_WhenUnsuccessful()
        {
            var userDto = new AdminUserCreationDTO { Email = "fail@email.com", Password = "badpass" };
            var expected = new APIResponse { Success = false };
            _mockUserService.Setup(x => x.InsertUser(It.IsAny<AdminUserCreationDTO>())).Returns(expected);

            var result = _controller.InsertUser(userDto);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            var api = Assert.IsType<APIResponse>(badRequest.Value);
            Assert.False(api.Success);
        }

        [Fact]
        public void GetAllUsers_ReturnsOkWithData()
        {
            var list = new ListUsers();
            _mockUserService.Setup(x => x.GetAllUsers()).Returns(list);

            var result = _controller.GetAllUsers();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<ListUsers>(okResult.Value);
            Assert.Equal(list, returned);
        }

        [Fact]
        public void GetAllChildren_ReturnsOkWithData()
        {
            var children = new ListChildren();
            _mockUserService.Setup(x => x.GetAllChildren()).Returns(children);

            var result = _controller.GetAllChildren();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<ListChildren>(okResult.Value);
            Assert.Equal(children, returned);
        }

        [Fact]
        public void UpdateUser_ReturnsOk_WhenSuccessful()
        {
            var updateDto = new AdminUserUpdateDTO();
            var response = new APIResponse { Success = true };
            _mockUserService.Setup(s => s.UpdateUser(updateDto)).Returns(response);

            var result = _controller.UpdateUser(updateDto);
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var api = Assert.IsType<APIResponse>(ok.Value);
            Assert.True(api.Success);
        }

        [Fact]
        public void UpdateUser_ReturnsBadRequest_WhenFails()
        {
            var updateDto = new AdminUserUpdateDTO();
            var response = new APIResponse { Success = false, Message = "Fail" };
            _mockUserService.Setup(s => s.UpdateUser(updateDto)).Returns(response);

            var result = _controller.UpdateUser(updateDto);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            var message = Assert.IsType<string>(badRequest.Value);
            Assert.Equal("Fail", message);
        }
    }
}