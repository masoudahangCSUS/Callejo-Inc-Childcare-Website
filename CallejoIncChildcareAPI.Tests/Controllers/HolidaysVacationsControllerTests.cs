using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Common.Services.SQL;
using Common.View;
using CallejoIncChildcareAPI.Controllers;
using Xunit;

namespace CallejoIncChildcareAPI.Tests
{
    public class HolidaysVacationsControllerTests
    {
        [Fact]
        public void GetHolidaysVacations_ReturnsOkResult_WithList()
        {
            // Arrange
            var mockService = new Mock<ISQLServices>();
            mockService.Setup(service => service.GetHolidaysVacations())
                .Returns(new List<HolidaysVacationView>
                {
            new HolidaysVacationView
            {
                Id = 1,
                Title = "Spring Break",
                Description = "Spring break for students and staff.",
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
                Type = "Vacation",
                CreatedAt = DateTime.UtcNow
            }
                });

            var controller = new HolidaysVacationsController(mockService.Object);

            // Act
            var actionResult = controller.GetHolidaysVacations().Result;

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var data = Assert.IsAssignableFrom<IEnumerable<HolidaysVacationView>>(okResult.Value);
            Assert.Single(data);
        }


        [Fact]
        public void CreateHolidayVacation_ReturnsOkResult_WhenHolidayType()
        {
            // Arrange
            var mockService = new Mock<ISQLServices>();
            var holiday = new HolidaysVacationView
            {
                Title = "MLK Day",
                Type = "Holiday",
                Description = "Martin Luther King Jr. Day observed.",
                StartDate = new DateOnly(2025, 1, 20),
                EndDate = new DateOnly(2025, 1, 20),
                CreatedAt = DateTime.UtcNow
            };
            mockService.Setup(service => service.CreateHolidayVacation(It.IsAny<HolidaysVacationView>())).Returns(true);

            var controller = new HolidaysVacationsController(mockService.Object);

            // Act
            var result = controller.CreateHolidayVacation(holiday);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Holiday/Vacation created successfully.", okResult.Value);
        }

        [Fact]
        public void CreateHolidayVacation_ReturnsOkResult_WhenVacationType()
        {
            // Arrange
            var mockService = new Mock<ISQLServices>();
            var vacation = new HolidaysVacationView
            {
                Title = "Summer Vacation",
                Type = "Vacation",
                Description = "Summer break for all students.",
                StartDate = new DateOnly(2025, 6, 10),
                EndDate = new DateOnly(2025, 6, 20),
                CreatedAt = DateTime.UtcNow
            };
            mockService.Setup(service => service.CreateHolidayVacation(It.IsAny<HolidaysVacationView>())).Returns(true);

            var controller = new HolidaysVacationsController(mockService.Object);

            // Act
            var result = controller.CreateHolidayVacation(vacation);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Holiday/Vacation created successfully.", okResult.Value);
        }

        [Fact]
        public void CreateHolidayVacation_ReturnsBadRequest_WhenTitleIsMissing()
        {
            // Arrange
            var controller = new HolidaysVacationsController(new Mock<ISQLServices>().Object);
            var invalidHoliday = new HolidaysVacationView { Title = "" };

            // Act
            var result = controller.CreateHolidayVacation(invalidHoliday);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateHolidayVacation_ReturnsOkResult_WhenSuccess()
        {
            // Arrange
            var mockService = new Mock<ISQLServices>();
            var updated = new HolidaysVacationView
            {
                Title = "Updated Spring Break",
                Type = "Vacation",
                Description = "Updated description for spring break.",
                StartDate = new DateOnly(2025, 4, 1),
                EndDate = new DateOnly(2025, 4, 5),
                CreatedAt = DateTime.UtcNow
            };
            mockService.Setup(service => service.UpdateHolidayVacation(It.IsAny<long>(), It.IsAny<HolidaysVacationView>())).Returns(true);

            var controller = new HolidaysVacationsController(mockService.Object);

            // Act
            var result = controller.UpdateHolidayVacation(1, updated);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Updated successfully.", okResult.Value);
        }

        [Fact]
        public void UpdateHolidayVacation_HandlesHolidayType_Correctly()
        {
            // Arrange
            var mockService = new Mock<ISQLServices>();
            var update = new HolidaysVacationView
            {
                Title = "Presidents' Day",
                Type = "Holiday",
                Description = "Holiday for Presidents' Day.",
                StartDate = new DateOnly(2025, 2, 17),
                EndDate = new DateOnly(2025, 2, 17),
                CreatedAt = DateTime.UtcNow
            };
            mockService.Setup(service => service.UpdateHolidayVacation(It.IsAny<long>(), It.IsAny<HolidaysVacationView>())).Returns(true);

            var controller = new HolidaysVacationsController(mockService.Object);

            // Act
            var result = controller.UpdateHolidayVacation(2, update);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Updated successfully.", okResult.Value);
        }

        [Fact]
        public void UpdateHolidayVacation_ReturnsNotFound_WhenInvalidId()
        {
            // Arrange
            var mockService = new Mock<ISQLServices>();
            mockService.Setup(service => service.UpdateHolidayVacation(It.IsAny<long>(), It.IsAny<HolidaysVacationView>())).Returns(false);

            var controller = new HolidaysVacationsController(mockService.Object);
            var update = new HolidaysVacationView { Title = "Test", Type = "Holiday" };

            // Act
            var result = controller.UpdateHolidayVacation(999, update);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Holiday/Vacation not found.", notFoundResult.Value);
        }

        [Fact]
        public void DeleteHolidayVacation_ReturnsOkResult_WhenSuccess()
        {
            // Arrange
            var mockService = new Mock<ISQLServices>();
            mockService.Setup(service => service.DeleteHolidayVacation(It.IsAny<long>())).Returns(true);

            var controller = new HolidaysVacationsController(mockService.Object);

            // Act
            var result = controller.DeleteHolidayVacation(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Deleted successfully.", okResult.Value);
        }

        [Fact]
        public void DeleteHolidayVacation_ReturnsNotFound_WhenFailure()
        {
            // Arrange
            var mockService = new Mock<ISQLServices>();
            mockService.Setup(service => service.DeleteHolidayVacation(It.IsAny<long>())).Returns(false);

            var controller = new HolidaysVacationsController(mockService.Object);

            // Act
            var result = controller.DeleteHolidayVacation(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Holiday/Vacation not found.", notFoundResult.Value);
        }
    }
}
