using CallejoIncChildcareAPI.Controllers;
using Common.Models.Data;
using Common.Services.SQL;
using Common.Services.User;
using Common.View;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CallejoIncChildcareAPI.Tests
{
    public class CustomerControllerTests
    {
        private readonly Mock<ISQLServices> _mockSqlServices;
        private readonly Mock<IUserService> _mockUserService;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _mockSqlServices = new Mock<ISQLServices>();
            _mockUserService = new Mock<IUserService>();
            _controller = new CustomerController(_mockSqlServices.Object, _mockUserService.Object);
        }

        [Fact]
        public void GetChildrenGuardian_ReturnsOkResult()
        {
            // Arrange
            var childrenGuardians = new ListChildrenGuardianView();
            _mockSqlServices.Setup(s => s.GetListOfAllChildrenAndGuardians())
                .Returns(childrenGuardians);

            // Act
            var result = _controller.GetChildrenGuardian();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(childrenGuardians, okResult.Value);
        }

        [Fact]
        public void InsertUser_ReturnsOkResult_WhenSuccess()
        {
            // Arrange
            var userInfo = new CustomerUserCreationDTO();
            var apiResponse = new APIResponse { Success = true };
            _mockUserService.Setup(s => s.InsertUser(userInfo)).Returns(apiResponse);

            // Act
            var result = _controller.InsertUser(userInfo);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(apiResponse, okResult.Value);
        }


        [Fact]
        public void InsertUser_ReturnsBadRequest_WhenFailure()
        {
            // Arrange
            var userInfo = new CustomerUserCreationDTO();
            var apiResponse = new APIResponse { Success = false };
            _mockUserService.Setup(s => s.InsertUser(userInfo)).Returns(apiResponse);

            // Act
            var result = _controller.InsertUser(userInfo);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(apiResponse, badRequestResult.Value);
        }

        [Fact]
        public async Task GetEmergencyContact_ReturnsOkResult_WhenContactExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var contact = new EmergencyContact();
            _mockUserService.Setup(s => s.GetEmergencyContactAsync(id)).ReturnsAsync(contact);
            _mockSqlServices.Setup(s => s.GetPhoneNumber(id, 3)).ReturnsAsync(new List<PhoneNumber> { new PhoneNumber() });
            _mockSqlServices.Setup(s => s.GetPhoneNumber(id, 4)).ReturnsAsync(new List<PhoneNumber> { new PhoneNumber() });

            // Act
            var result = await _controller.GetEmergencyContact(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task GetEmergencyContact_ReturnsNotFound_WhenContactDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockUserService.Setup(s => s.GetEmergencyContactAsync(id)).ReturnsAsync((EmergencyContact)null);

            // Act
            var result = await _controller.GetEmergencyContact(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Emergency contact not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetPhoneNumber_ReturnsOkResult_WhenPhoneNumberExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var type = 1L;
            var phoneNumber = new PhoneNumber();
            _mockSqlServices.Setup(s => s.GetPhoneNumber(id, type)).ReturnsAsync(new List<PhoneNumber> { phoneNumber });

            // Act
            var result = await _controller.GetPhoneNumber(id, type);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(phoneNumber, okResult.Value);
        }

        [Fact]
        public async Task GetPhoneNumber_ReturnsBadRequest_WhenPhoneNumberDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var type = 1L;
            _mockSqlServices.Setup(s => s.GetPhoneNumber(id, type)).ReturnsAsync(new List<PhoneNumber>());

            // Act
            var result = await _controller.GetPhoneNumber(id, type);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Null(badRequestResult.Value);
        }

        [Fact]
        public async Task GetUserByID_ReturnsOkResult_WhenUserExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var user = new CallejoIncUser();
            _mockUserService.Setup(s => s.GetUserByID(id)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUserByID(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task GetUserByID_ReturnsBadRequest_WhenUserDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockUserService.Setup(s => s.GetUserByID(id)).ReturnsAsync((CallejoIncUser)null);

            // Act
            var result = await _controller.GetUserByID(id);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User not found.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetChildList_ReturnsOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var children = new List<Child>();
            _mockSqlServices.Setup(s => s.GetChildren(id)).ReturnsAsync(children);

            // Act
            var result = await _controller.GetChildList(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(children, okResult.Value);
        }

        [Fact]
        public async Task GetChildrenByID_ReturnsOkResult_WhenChildExists()
        {
            // Arrange
            var id = 1L;
            var child = new Child();
            _mockSqlServices.Setup(s => s.getChildById(id)).ReturnsAsync(child);

            // Act
            var result = await _controller.GetChildrenByID(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task GetChildrenByID_ReturnsNotFound_WhenChildDoesNotExist()
        {
            // Arrange
            var id = 1L;
            _mockSqlServices.Setup(s => s.getChildById(id)).ReturnsAsync((Child)null);

            // Act
            var result = await _controller.GetChildrenByID(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Child not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateUser_ReturnsOkResult_WhenUpdateSuccessful()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDto = new CustomerUserViewDTO();
            var user = new CallejoIncUser();
            _mockSqlServices.Setup(s => s.getUserWithNumber(userId)).ReturnsAsync(user);
            _mockSqlServices.Setup(s => s.updateUser(user, userDto)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateUser(userId, userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User Updated Succesfully", okResult.Value);
        }

        [Fact]
        public async Task UpdateUser_ReturnsBadRequest_WhenUpdateFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDto = new CustomerUserViewDTO();
            var user = new CallejoIncUser();
            _mockSqlServices.Setup(s => s.getUserWithNumber(userId)).ReturnsAsync(user);
            _mockSqlServices.Setup(s => s.updateUser(user, userDto)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateUser(userId, userDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User Update Failed", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateEmergencyContact_ReturnsOkResult_WhenUpdateSuccessful()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var emergencyDto = new EmergencyContactDTO();
            var emergencyContact = new EmergencyContact();
            _mockUserService.Setup(s => s.GetEmergencyContactAsync(userId)).ReturnsAsync(emergencyContact);
            _mockSqlServices.Setup(s => s.updateEmergencyContact(emergencyContact, emergencyDto)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateEmergencyContact(userId, emergencyDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Emergency Contact succesfully updated", okResult.Value);
        }

        [Fact]
        public async Task UpdateEmergencyContact_ReturnsBadRequest_WhenUpdateFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var emergencyDto = new EmergencyContactDTO();
            var emergencyContact = new EmergencyContact();
            _mockUserService.Setup(s => s.GetEmergencyContactAsync(userId)).ReturnsAsync(emergencyContact);
            _mockSqlServices.Setup(s => s.updateEmergencyContact(emergencyContact, emergencyDto)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateEmergencyContact(userId, emergencyDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Emergency Contact update failed", badRequestResult.Value);
        }
        [Fact]
        public async Task UpdateChild_ReturnsOkResult_WhenUpdateSuccessful()
        {
            // Arrange
            var childId = 1L;
            var childDto = new ChildDTO();
            var child = new Child();
            _mockSqlServices.Setup(s => s.getChildById(childId)).ReturnsAsync(child);
            _mockSqlServices.Setup(s => s.updateChild(child, childDto)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateChild(childId, childDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Child updated successfully.", okResult.Value);
        }

        [Fact]
        public async Task UpdateChild_ReturnsBadRequest_WhenChildDtoIsNull()
        {
            // Arrange
            var childId = 1L;
            ChildDTO childDto = null;

            // Act
            var result = await _controller.UpdateChild(childId, childDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Child data is null.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateChild_ReturnsNotFound_WhenChildDoesNotExist()
        {
            // Arrange
            var childId = 1L;
            var childDto = new ChildDTO();
            _mockSqlServices.Setup(s => s.getChildById(childId)).ReturnsAsync((Child)null);

            // Act
            var result = await _controller.UpdateChild(childId, childDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Child not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateChild_ReturnsBadRequest_WhenUpdateFails()
        {
            // Arrange
            var childId = 1L;
            var childDto = new ChildDTO();
            var child = new Child();
            _mockSqlServices.Setup(s => s.getChildById(childId)).ReturnsAsync(child);
            _mockSqlServices.Setup(s => s.updateChild(child, childDto)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateChild(childId, childDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Child update failed.", badRequestResult.Value);
        }
        [Fact]
        public async Task UpdatePassword_ReturnsOkResult_WhenUpdateSuccessful()
        {
            // Arrange
            var settings = new SettingsDTO { Id = Guid.NewGuid() };
            _mockSqlServices.Setup(s => s.updatePassowrd(settings)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdatePassword(settings);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Password updated successfully.", okResult.Value);
        }

        [Fact]
        public async Task UpdatePassword_ReturnsBadRequest_WhenSettingsIsNull()
        {
            // Arrange
            SettingsDTO settings = null;

            // Act
            var result = await _controller.UpdatePassword(settings);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid settings data provided.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdatePassword_ReturnsBadRequest_WhenSettingsIdIsEmpty()
        {
            // Arrange
            var settings = new SettingsDTO { Id = Guid.Empty };

            // Act
            var result = await _controller.UpdatePassword(settings);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid settings data provided.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdatePassword_ReturnsStatusCode500_WhenUpdateFails()
        {
            // Arrange
            var settings = new SettingsDTO { Id = Guid.NewGuid() };
            _mockSqlServices.Setup(s => s.updatePassowrd(settings)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdatePassword(settings);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error has ocurred whule updating the password", statusCodeResult.Value);
        }
        [Fact]
        public async Task UpdateEmail_ReturnsOkResult_WhenUpdateSuccessful()
        {
            // Arrange
            var settings = new SettingsDTO { Id = Guid.NewGuid() };
            _mockSqlServices.Setup(s => s.updateEmail(settings)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateEmail(settings);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Email updated successfully.", okResult.Value);
        }

        [Fact]
        public async Task UpdateEmail_ReturnsBadRequest_WhenSettingsIsNull()
        {
            // Arrange
            SettingsDTO settings = null;

            // Act
            var result = await _controller.UpdateEmail(settings);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid settings data provided.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateEmail_ReturnsBadRequest_WhenSettingsIdIsEmpty()
        {
            // Arrange
            var settings = new SettingsDTO { Id = Guid.Empty };

            // Act
            var result = await _controller.UpdateEmail(settings);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid settings data provided.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateEmail_ReturnsStatusCode500_WhenUpdateFails()
        {
            // Arrange
            var settings = new SettingsDTO { Id = Guid.NewGuid() };
            _mockSqlServices.Setup(s => s.updateEmail(settings)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateEmail(settings);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error has ocurred while updating the passowrd", statusCodeResult.Value);
        }

    }
}