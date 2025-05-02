using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Common.Models.Data;
using Common.Services.Submit;
using CallejoIncChildcareAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CallejoIncChildcareAPI.Tests
{
    public class SubmitControllerTests
    {
        [Fact]
        public async Task SubmitForm_ReturnsOk_WithInquiry()
        {
            // Arrange
            var inquiry = new InterestedParent
            {
                Id = Guid.NewGuid(),
                Name = "Test Parent",
                Email = "parent@example.com",
                Phone = "123-456-7890",
                ReasonForInquiry = "Enrollment",
                Datetime = DateTime.UtcNow
            };

            var mockService = new Mock<ISubmitService>();
            mockService.Setup(s => s.AddInquiryAsync(It.IsAny<InterestedParent>()))
                       .Returns(Task.CompletedTask);

            var controller = new SubmitController(mockService.Object);

            // Act
            var result = await controller.SubmitForm(inquiry);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(inquiry, okResult.Value);
        }

        [Fact]
        public async Task GetFormData_ReturnsOk_WithData()
        {
            var mockData = new List<InterestedParent>
            {
                new InterestedParent { Name = "A", Email = "a@a.com", Phone = "111", ReasonForInquiry = "Info", Datetime = DateTime.UtcNow }
            };

            var mockService = new Mock<ISubmitService>();
            mockService.Setup(s => s.GetInquiryAsync()).ReturnsAsync(mockData);

            var controller = new SubmitController(mockService.Object);

            var result = await controller.GetFormData();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<List<InterestedParent>>(okResult.Value);
            Assert.Single(returned);
        }

        [Fact]
        public async Task DeleteInquiry_ReturnsNotFound_WhenMissing()
        {
            var mockService = new Mock<ISubmitService>();
            mockService.Setup(s => s.DeleteInquiryAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            var controller = new SubmitController(mockService.Object);

            var result = await controller.DeleteInquiry(Guid.NewGuid());

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteInquiry_ReturnsOk_WhenDeleted()
        {
            var mockService = new Mock<ISubmitService>();
            mockService.Setup(s => s.DeleteInquiryAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            var controller = new SubmitController(mockService.Object);

            var result = await controller.DeleteInquiry(Guid.NewGuid());

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Inquiry deleted successfully.", okResult.Value);
        }
    }
}
