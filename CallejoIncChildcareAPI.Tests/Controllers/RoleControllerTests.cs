using CallejoIncChildcareAPI.Controllers;
using Common.Services.Login;
using Common.Services.Role;
using Common.View;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CallejoIncChildcareAPI.Tests
{
    public class RoleControllerTests
    {
        private readonly Mock<IRoleService> _mockRoleService;
        private readonly Mock<ILoginService> _mockLoginService;
        private readonly RoleController _controller;

        public RoleControllerTests()
        {
            _mockRoleService = new Mock<IRoleService>();
            _mockLoginService = new Mock<ILoginService>();
            _controller = new RoleController(_mockRoleService.Object, _mockLoginService.Object);
        }

        [Fact]
        public void GetAllRoles_ReturnsOkResult_WithListRoles()
        {
            // Arrange
            var roles = new ListRoles { roles = new List<RoleView> { new RoleView { Id = 1, Description = "Admin" } } };
            _mockRoleService.Setup(service => service.GetAllRoles()).Returns(roles);

            // Act
            var result = _controller.GetAllRoles();

            // Assert
            var actionResult = Assert.IsType<ActionResult<ListRoles>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<ListRoles>(okResult.Value);
            Assert.Equal(roles.roles.Count, returnValue.roles.Count);
        }

        [Fact]
        public void GetRole_ReturnsOkResult_WhenRoleExists()
        {
            // Arrange
            var role = new ListRoles { roles = new List<RoleView> { new RoleView { Id = 1, Description = "Admin" } }, Success = true };
            _mockRoleService.Setup(service => service.GetRole(1)).Returns(role);

            // Act
            var result = _controller.GetRole(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ListRoles>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<ListRoles>(okResult.Value);
            Assert.True(returnValue.Success);
        }

        [Fact]
        public void GetRole_ReturnsBadRequest_WhenRoleDoesNotExist()
        {
            // Arrange
            var role = new ListRoles { Success = false };
            _mockRoleService.Setup(service => service.GetRole(1)).Returns(role);

            // Act
            var result = _controller.GetRole(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ListRoles>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<ListRoles>(badRequestResult.Value);
            Assert.False(returnValue.Success);
        }

        [Fact]
        public void InsertRole_ReturnsOkResult_WhenInsertIsSuccessful()
        {
            // Arrange
            var response = new APIResponse { Success = true };
            var roleInfo = new RoleView { Id = 1, Description = "Admin" };
            _mockRoleService.Setup(service => service.InsertRole(roleInfo)).Returns(response);

            // Act
            var result = _controller.InsertRole(roleInfo);

            // Assert
            var actionResult = Assert.IsType<ActionResult<APIResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<APIResponse>(okResult.Value);
            Assert.True(returnValue.Success);
        }

        [Fact]
        public void InsertRole_ReturnsBadRequest_WhenInsertFails()
        {
            // Arrange
            var response = new APIResponse { Success = false };
            var roleInfo = new RoleView { Id = 1, Description = "Admin" };
            _mockRoleService.Setup(service => service.InsertRole(roleInfo)).Returns(response);

            // Act
            var result = _controller.InsertRole(roleInfo);

            // Assert
            var actionResult = Assert.IsType<ActionResult<APIResponse>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<APIResponse>(badRequestResult.Value);
            Assert.False(returnValue.Success);
        }

        [Fact]
        public void UpdateRole_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            var response = new APIResponse { Success = true };
            var roleInfo = new RoleView { Id = 1, Description = "Admin" };
            _mockRoleService.Setup(service => service.UpdateRole(roleInfo)).Returns(response);

            // Act
            var result = _controller.UpdateRole(roleInfo);

            // Assert
            var actionResult = Assert.IsType<ActionResult<APIResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<APIResponse>(okResult.Value);
            Assert.True(returnValue.Success);
        }

        [Fact]
        public void UpdateRole_ReturnsBadRequest_WhenUpdateFails()
        {
            // Arrange
            var response = new APIResponse { Success = false };
            var roleInfo = new RoleView { Id = 1, Description = "Admin" };
            _mockRoleService.Setup(service => service.UpdateRole(roleInfo)).Returns(response);

            // Act
            var result = _controller.UpdateRole(roleInfo);

            // Assert
            var actionResult = Assert.IsType<ActionResult<APIResponse>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<APIResponse>(badRequestResult.Value);
            Assert.False(returnValue.Success);
        }

        [Fact]
        public void DeleteRole_ReturnsOkResult_WhenDeleteIsSuccessful()
        {
            // Arrange
            var response = new APIResponse { Success = true };
            _mockRoleService.Setup(service => service.DeleteRole(1)).Returns(response);

            // Act
            var result = _controller.DeleteRole(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<APIResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<APIResponse>(okResult.Value);
            Assert.True(returnValue.Success);
        }

        [Fact]
        public void DeleteRole_ReturnsBadRequest_WhenDeleteFails()
        {
            // Arrange
            var response = new APIResponse { Success = false };
            _mockRoleService.Setup(service => service.DeleteRole(1)).Returns(response);

            // Act
            var result = _controller.DeleteRole(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<APIResponse>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<APIResponse>(badRequestResult.Value);
            Assert.False(returnValue.Success);
        }
    }
}
