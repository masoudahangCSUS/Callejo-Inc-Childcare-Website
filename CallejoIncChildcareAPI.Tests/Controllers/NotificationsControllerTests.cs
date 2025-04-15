using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Common.Services.SQL;
using Common.View;
using CallejoIncChildcareAPI.Controllers;
using Xunit;
using System.Linq;
using Common.Services.Login;

namespace CallejoIncChildcareAPI.Tests
{
    public class NotificationsControllerTests
    {
        private readonly Mock<ISQLServices> _mockService;
        private Mock<ILoginService> _mockLoginService;
        private readonly NotificationsController _controller;

        public NotificationsControllerTests()
        {
            _mockService = new Mock<ISQLServices>();
            _mockLoginService = new Mock<ILoginService>();
            _controller = new NotificationsController(_mockService.Object, _mockLoginService.Object);
        }

        [Fact]
        public void GetNotificationsByParentId_ReturnsOk_WhenDataExists()
        {
            // Arrange
            ReturnAuthorized();
            var parentId = Guid.NewGuid();
            var mockData = new List<NotificationView> { new NotificationView { Id = 1, Message = "Test Message" } };
            _mockService.Setup(s => s.GetNotificationsByParentId(parentId)).Returns(mockData);

            var result = _controller.GetNotificationsByParentId(parentId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnData = Assert.IsAssignableFrom<IEnumerable<NotificationView>>(okResult.Value);
            Assert.Single(returnData);
        }

        [Fact]
        public void GetNotificationsByParentId_ReturnsNotFound_WhenNoData()
        {
            // Arrange
            ReturnAuthorized();
            var parentId = Guid.NewGuid();
            _mockService.Setup(s => s.GetNotificationsByParentId(parentId)).Returns(new List<NotificationView>());

            var result = _controller.GetNotificationsByParentId(parentId);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetAllNotifications_ReturnsOk_WhenDataExists()
        {
            // Arrange
            ReturnAuthorized();
            _mockService.Setup(s => s.GetAllNotifications()).Returns(new List<NotificationView> { new NotificationView() });

            var result = _controller.GetAllNotifications();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public void GetAllNotifications_ReturnsNotFound_WhenNoData()
        {
            // Arrange
            ReturnAuthorized();
            _mockService.Setup(s => s.GetAllNotifications()).Returns(new List<NotificationView>());

            var result = _controller.GetAllNotifications();

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void MarkNotificationAsRead_ReturnsOk_WhenSuccess()
        {
            // Arrange
            ReturnAuthorized();
            _mockService.Setup(s => s.MarkNotificationAsRead(1)).Returns(true);

            var result = _controller.MarkNotificationAsRead(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Notification marked as read.", ok.Value);
        }

        [Fact]
        public void MarkNotificationAsRead_ReturnsNotFound_WhenFail()
        {
            // Arrange
            ReturnAuthorized();
            _mockService.Setup(s => s.MarkNotificationAsRead(1)).Returns(false);

            var result = _controller.MarkNotificationAsRead(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CreateNotification_ReturnsOk_WhenValid()
        {
            // Arrange
            ReturnAuthorized();
            var notification = new NotificationView { Title = "Title", Message = "Msg", FkParentId = Guid.NewGuid() };
            _mockService.Setup(s => s.CreateNotification(It.IsAny<NotificationView>())).Returns(true);

            var result = _controller.CreateNotification(notification);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Notification created successfully.", ok.Value);
        }

        [Fact]
        public void CreateNotification_ReturnsBadRequest_WhenMissingFields()
        {
            // Arrange
            _mockLoginService.Setup(service => service.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(true); // Ensure authentication is successful
            var invalid = new NotificationView { Message = "" };

            var result = _controller.CreateNotification(invalid);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateNotification_ReturnsServerError_WhenInsertFails()
        {
            // Arrange
            _mockLoginService.Setup(service => service.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(true);
            var notification = new NotificationView { Title = "T", Message = "M", FkParentId = Guid.NewGuid() };
            _mockService.Setup(s => s.CreateNotification(It.IsAny<NotificationView>())).Returns(false);

            var result = _controller.CreateNotification(notification);

            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public void UpdateNotification_ReturnsOk_WhenValid()
        {
            // Arrange
            _mockLoginService.Setup(service => service.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(true);
            var updated = new NotificationView { Title = "Updated", Message = "Updated Msg", SentOn = DateTime.UtcNow };
            _mockService.Setup(s => s.UpdateNotification(1, updated)).Returns(true);

            var result = _controller.UpdateNotification(1, updated);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Notification updated successfully.", ok.Value);
        }

        [Fact]
        public void UpdateNotification_ReturnsBadRequest_WhenInvalidData()
        {
            // Arrange
            _mockLoginService.Setup(service => service.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(true);
            var invalid = new NotificationView { Message = "" };

            var result = _controller.UpdateNotification(1, invalid);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateNotification_ReturnsNotFound_WhenNotExists()
        {
            // Arrange
            _mockLoginService.Setup(service => service.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(true);
            var update = new NotificationView { Message = "Valid" };
            _mockService.Setup(s => s.UpdateNotification(1, update)).Returns(false);

            var result = _controller.UpdateNotification(1, update);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void DeleteNotification_ReturnsOk_WhenSuccess()
        {
            // Arrange
            _mockLoginService.Setup(service => service.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(true);
            _mockService.Setup(s => s.DeleteNotification(1)).Returns(true);

            var result = _controller.DeleteNotification(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Notification deleted successfully.", ok.Value);
        }

        [Fact]
        public void DeleteNotification_ReturnsNotFound_WhenNotExists()
        {
            // Arrange
            _mockLoginService.Setup(service => service.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(true);
            _mockService.Setup(s => s.DeleteNotification(1)).Returns(false);
            // Act
            var result = _controller.DeleteNotification(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void SendCustomNotification_ReturnsBadRequest_WhenMessageIsEmpty()
        {
            // Arrange
            _mockLoginService.Setup(service => service.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(true);
            var mockService = new Mock<ISQLServices>();
            var controller = new NotificationsController(mockService.Object, this._mockLoginService.Object);
            var invalidNotification = new NotificationView
            {
                FkParentId = Guid.NewGuid(),
                Title = "Test",
                Message = "" // Empty message
            };

            // Act
            var result = controller.SendCustomNotification(invalidNotification);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid notification data.", badRequestResult.Value);
        }

        [Fact]
        public void SendCustomNotification_ReturnsOk_WhenSuccess()
        {
            // Arrange
            _mockLoginService.Setup(service => service.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(true);
            var mockService = new Mock<ISQLServices>();
            mockService.Setup(service => service.SendCustomNotification(It.IsAny<NotificationView>()))
                       .Returns(true);

            var controller = new NotificationsController(mockService.Object, this._mockLoginService.Object);
            var validNotification = new NotificationView
            {
                FkParentId = Guid.NewGuid(),
                Title = "Parent Request",
                Message = "Please update my child info."
            };

            // Act
            var result = controller.SendCustomNotification(validNotification);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Notification sent successfully.", okResult.Value);
        }

        [Fact]
        public void SendCustomNotification_ReturnsServerError_WhenInsertFails()
        {
            // Arrange
            _mockLoginService.Setup(service => service.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(true);
            var mockService = new Mock<ISQLServices>();
            mockService.Setup(service => service.SendCustomNotification(It.IsAny<NotificationView>()))
                       .Returns(false); // simulate DB failure

            var controller = new NotificationsController(mockService.Object, this._mockLoginService.Object);
            var failedNotification = new NotificationView
            {
                FkParentId = Guid.NewGuid(),
                Title = "Feedback",
                Message = "I have a concern."
            };

            // Act
            var result = controller.SendCustomNotification(failedNotification);

            // Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
            Assert.Equal("Failed to save the notification.", serverError.Value);
        }
        private void ReturnAuthorized()
        {
            // Arrange
            //_mockLoginService = new Mock<ILoginService>();
            //_mockLoginService.Setup(service => service.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<Guid>()))
            //    .Returns(true);
            // Arrange
            _mockLoginService.Setup(service => service.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(true); // Ensure authentication is successful
        }

    }
}
