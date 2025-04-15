using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Common.Services.DailySchedule;
using Common.View;
using CallejoIncChildcareAPI.Controllers;
using Xunit;
using Common.Services.Login;
using System.Collections;

namespace CallejoIncChildcareAPI.Tests
{
    public class DailyScheduleControllerTests
    {
        [Fact]
        public void GetDailyScheduleByDate_ReturnsUnauthorizedResult_WhenUserNotAuthenticated()
        {
            // Arrange
            var mockLoginService = new Mock<ILoginService>();
            mockLoginService.Setup(service => service.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(false);

            var mockService = new Mock<IDailyScheduleService>();
            mockService.Setup(service => service.GetDailyScheduleByDate(It.IsAny<DateOnly>()))
                .Returns(new ListDailySchedule { Success = true, Data = new List<object> { new { Id = 1, Name = "Schedule" } } });

            var controller = new DailyScheduleController(mockService.Object, mockLoginService.Object);
            var testDate = DateOnly.FromDateTime(DateTime.Now);

            // Act
            var result = controller.GetDailyScheduleByDate(testDate);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
            var response = Assert.IsType<APIResponse>(unauthorizedResult.Value);
            Assert.False(response.Success);
            Assert.Equal("User is not authenticated.", response.Message);
        }

        [Fact]
        public void GetDailyScheduleByDate_ReturnsOkResult_WhenUserAuthenticatedAndServiceSuccess()
        {
            // Arrange
            var mockLoginService = new Mock<ILoginService>();
            mockLoginService.Setup(service => service.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(true);

            var mockService = new Mock<IDailyScheduleService>();
            mockService.Setup(service => service.GetDailyScheduleByDate(It.IsAny<DateOnly>()))
                .Returns(new ListDailySchedule { Success = true, Data = new List<object> { new { Id = 1, Name = "Schedule" } } });

            var controller = new DailyScheduleController(mockService.Object, mockLoginService.Object);
            var testDate = DateOnly.FromDateTime(DateTime.Now);

            // Act
            var result = controller.GetDailyScheduleByDate(testDate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var data = Assert.IsType<ListDailySchedule>(okResult.Value);
            Assert.True(data.Success);
            Assert.NotEmpty((IEnumerable)data.Data);
        }
        [Fact]
        public void GetDailyScheduleById_ReturnsUnauthorizedResult_WhenUserNotAuthenticated()
        {
            // Arrange
            var mockLoginService = new Mock<ILoginService>();
            mockLoginService.Setup(service => service.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(false);

            var mockService = new Mock<IDailyScheduleService>();
            mockService.Setup(service => service.GetDailyScheduleById(It.IsAny<long>()))
                .Returns(new ListDailySchedule { Success = true, Data = new List<object> { new { Id = 1, Name = "Schedule" } } });

            var controller = new DailyScheduleController(mockService.Object, mockLoginService.Object);
            var testId = 1L;

            // Act
            var result = controller.GetDailyScheduleById(testId);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
            var response = Assert.IsType<APIResponse>(unauthorizedResult.Value);
            Assert.False(response.Success);
            Assert.Equal("User is not authenticated.", response.Message);
        }

        [Fact]
        public void GetDailyScheduleById_ReturnsOkResult_WhenUserAuthenticatedAndServiceSuccess()
        {
            // Arrange
            var mockLoginService = new Mock<ILoginService>();
            mockLoginService.Setup(service => service.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(true);

            var mockService = new Mock<IDailyScheduleService>();
            mockService.Setup(service => service.GetDailyScheduleById(It.IsAny<long>()))
                .Returns(new ListDailySchedule { Success = true, Data = new List<object> { new { Id = 1, Name = "Schedule" } } });

            var controller = new DailyScheduleController(mockService.Object, mockLoginService.Object);
            var testId = 1L;

            // Act
            var result = controller.GetDailyScheduleById(testId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var data = Assert.IsType<ListDailySchedule>(okResult.Value);
            Assert.True(data.Success);
            Assert.NotEmpty((IEnumerable)data.Data);
        }

        [Fact]
        public void GetDailyScheduleById_ReturnsBadRequestResult_WhenServiceFails()
        {
            // Arrange
            var mockLoginService = new Mock<ILoginService>();
            mockLoginService.Setup(service => service.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(true);

            var mockService = new Mock<IDailyScheduleService>();
            mockService.Setup(service => service.GetDailyScheduleById(It.IsAny<long>()))
                .Returns(new ListDailySchedule { Success = false, Data = null });

            var controller = new DailyScheduleController(mockService.Object, mockLoginService.Object);
            var testId = 1L;

            // Act
            var result = controller.GetDailyScheduleById(testId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var response = Assert.IsType<ListDailySchedule>(badRequestResult.Value);
            Assert.False(response.Success);
        }
        [Fact]
        public void InsertDailySchedule_ReturnsUnauthorizedResult_WhenUserNotAuthenticated()
        {
            // Arrange
            var mockLoginService = new Mock<ILoginService>();
            mockLoginService.Setup(service => service.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(false);

            var mockService = new Mock<IDailyScheduleService>();
            var dailyScheduleView = new DailyScheduleView
            {
                Id = 1,
                Description = "Test Schedule",
                Desc_special = "Special Notes",
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            };

            var controller = new DailyScheduleController(mockService.Object, mockLoginService.Object);

            // Act
            var result = controller.InsertDailySchedule(dailyScheduleView);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
            var response = Assert.IsType<APIResponse>(unauthorizedResult.Value);
            Assert.False(response.Success);
            Assert.Equal("User is not authenticated.", response.Message);
        }

        [Fact]
        public void InsertDailySchedule_ReturnsOkResult_WhenUserAuthenticatedAndServiceSuccess()
        {
            // Arrange
            var mockLoginService = new Mock<ILoginService>();
            mockLoginService.Setup(service => service.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(true);

            var mockService = new Mock<IDailyScheduleService>();
            var dailyScheduleView = new DailyScheduleView
            {
                Id = 1,
                Description = "Test Schedule",
                Desc_special = "Special Notes",
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            };
            mockService.Setup(service => service.InsertDailySchedule(dailyScheduleView))
                .Returns(new APIResponse { Success = true });

            var controller = new DailyScheduleController(mockService.Object, mockLoginService.Object);

            // Act
            var result = controller.InsertDailySchedule(dailyScheduleView);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<APIResponse>(okResult.Value);
            Assert.True(response.Success);
        }

        [Fact]
        public void InsertDailySchedule_ReturnsBadRequestResult_WhenServiceFails()
        {
            // Arrange
            var mockLoginService = new Mock<ILoginService>();
            mockLoginService.Setup(service => service.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(true);

            var mockService = new Mock<IDailyScheduleService>();
            var dailyScheduleView = new DailyScheduleView
            {
                Id = 1,
                Description = "Test Schedule",
                Desc_special = "Special Notes",
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            };
            mockService.Setup(service => service.InsertDailySchedule(dailyScheduleView))
                .Returns(new APIResponse { Success = false });

            var controller = new DailyScheduleController(mockService.Object, mockLoginService.Object);

            // Act
            var result = controller.InsertDailySchedule(dailyScheduleView);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var response = Assert.IsType<APIResponse>(badRequestResult.Value);
            Assert.False(response.Success);
        }


    }

    //    [Fact]
    //    public void GetDailyScheduleByDate_ReturnsBadRequest_WhenFailure()
    //    {
    //        // Arrange
    //        var mockService = new Mock<IDailyScheduleService>();
    //        mockService.Setup(service => service.GetDailyScheduleByDate(It.IsAny<DateOnly>()))
    //            .Returns(new ListDailySchedule { Success = false });

    //        var controller = new DailyScheduleController(mockService.Object);
    //        var testDate = DateOnly.FromDateTime(DateTime.Now);

    //        // Act
    //        var result = controller.GetDailyScheduleByDate(testDate);

    //        // Assert
    //        Assert.IsType<BadRequestObjectResult>(result.Result);
    //    }

    //    [Fact]
    //    public void GetDailyScheduleById_ReturnsOkResult_WhenSuccess()
    //    {
    //        // Arrange
    //        var mockService = new Mock<IDailyScheduleService>();
    //        mockService.Setup(service => service.GetDailyScheduleById(It.IsAny<long>()))
    //            .Returns(new ListDailySchedule { Success = true, Data = new List<object> { new { Id = 1, Name = "Schedule" } } });

    //        var controller = new DailyScheduleController(mockService.Object);
    //        var testId = 1;

    //        // Act
    //        var result = controller.GetDailyScheduleById(testId);

    //        // Assert
    //        var okResult = Assert.IsType<OkObjectResult>(result.Result);
    //        var data = Assert.IsType<ListDailySchedule>(okResult.Value);
    //        Assert.True(data.Success);
    //    }

    //    [Fact]
    //    public void GetDailyScheduleById_ReturnsBadRequest_WhenFailure()
    //    {
    //        // Arrange
    //        var mockService = new Mock<IDailyScheduleService>();
    //        mockService.Setup(service => service.GetDailyScheduleById(It.IsAny<long>()))
    //            .Returns(new ListDailySchedule { Success = false });

    //        var controller = new DailyScheduleController(mockService.Object);
    //        var testId = 1;

    //        // Act
    //        var result = controller.GetDailyScheduleById(testId);

    //        // Assert
    //        Assert.IsType<BadRequestObjectResult>(result.Result);
    //    }

    //    [Fact]
    //    public void InsertDailySchedule_ReturnsOkResult_WhenSuccess()
    //    {
    //        // Arrange
    //        var mockService = new Mock<IDailyScheduleService>();
    //        var dailyScheduleView = new DailyScheduleView
    //        {
    //            Id = 2,
    //            Description = "Invalid Schedule",
    //            Desc_special = null,
    //            CreatedAt = DateOnly.FromDateTime(DateTime.Now)
    //        };
    //        var expectedResponse = new APIResponse { Success = true, Message = "Insert successful" };

    //        mockService.Setup(service => service.InsertDailySchedule(dailyScheduleView))
    //                   .Returns(expectedResponse);

    //        var controller = new DailyScheduleController(mockService.Object);

    //        // Act
    //        var result = controller.InsertDailySchedule(dailyScheduleView);

    //        // Assert
    //        var okResult = Assert.IsType<OkObjectResult>(result.Result);
    //        var response = Assert.IsType<APIResponse>(okResult.Value);
    //        Assert.True(response.Success);
    //        Assert.Equal("Insert successful", response.Message);
    //    }

    //    [Fact]
    //    public void InsertDailySchedule_ReturnsBadRequest_WhenFailure()
    //    {
    //        // Arrange
    //        var mockService = new Mock<IDailyScheduleService>();
    //        var dailyScheduleView = new DailyScheduleView
    //        {
    //            Id = 2,
    //            Description = "Afternoon Schedule",
    //            Desc_special = "Outdoor games",
    //            CreatedAt = DateOnly.FromDateTime(DateTime.Now.AddDays(-1))
    //        };
    //        var expectedResponse = new APIResponse { Success = false, Message = "Insert failed" };

    //        mockService.Setup(service => service.InsertDailySchedule(dailyScheduleView))
    //                   .Returns(expectedResponse);

    //        var controller = new DailyScheduleController(mockService.Object);

    //        // Act
    //        var result = controller.InsertDailySchedule(dailyScheduleView);

    //        // Assert
    //        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
    //        var response = Assert.IsType<APIResponse>(badRequestResult.Value);
    //        Assert.False(response.Success);
    //        Assert.Equal("Insert failed", response.Message);
    //    }
    //}
}
