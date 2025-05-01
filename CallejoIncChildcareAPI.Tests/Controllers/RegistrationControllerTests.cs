using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Common.Services.Registration;
using Common.View;
using CallejoIncChildcareAPI.Controllers;
using Microsoft.AspNetCore.Http;
using System.Text;
using Common.Models.Data;

namespace CallejoIncChildcareAPI.Tests
{
    public class RegistrationControllerTests
    {
        private static IFormFile CreateFakePdfFile()
        {
            var content = "Fake PDF content";
            var fileName = "test.pdf";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            return new FormFile(stream, 0, stream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };
        }

        private readonly Guid _testUserId = Guid.NewGuid(); // Fake GUID for testing purposes

        // ===== Upload Tests =====
        [Fact]
        public async Task UploadFile_ReturnsOk_WhenUploadSuccessful()
        {
            // Arrange
            var mockService = new Mock<IRegService>();
            var file = CreateFakePdfFile();
            mockService.Setup(service => service.UploadFileAsync(
                It.IsAny<Guid>(),
                It.IsAny<byte[]>(),
                It.IsAny<string>(),
                It.IsAny<long>()))
                .ReturnsAsync(true);

            var controller = new RegistrationController(mockService.Object);

            // Act
            var result = await controller.UploadFile(_testUserId, file);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("File uploaded successfully.", okResult.Value);
        }

        [Fact]
        public async Task UploadFile_ReturnsBadRequest_WhenFileIsNull()
        {
            // Arrange
            var mockService = new Mock<IRegService>();
            var controller = new RegistrationController(mockService.Object);

            // Act
            var result = await controller.UploadFile(_testUserId, null);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No file was uploaded.", badRequest.Value);
        }

        [Fact]
        public async Task UploadFile_ReturnsBadRequest_WhenFileIsNotPdf()
        {
            // Arrange
            var mockService = new Mock<IRegService>();
            var stream = new MemoryStream(Encoding.UTF8.GetBytes("Fake Content"));
            var file = new FormFile(stream, 0, stream.Length, "file", "test.txt")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain"
            };

            var controller = new RegistrationController(mockService.Object);

            // Act
            var result = await controller.UploadFile(_testUserId, file);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Only PDF files are allowed", badRequest.Value);
        }

        [Fact]
        public async Task UploadFile_ReturnsBadRequest_WhenFileTooLarge()
        {
            // Arrange
            var mockService = new Mock<IRegService>();
            var file = new FormFile(new MemoryStream(new byte[6 * 1024 * 1024]), 0, 6 * 1024 * 1024, "file", "test.pdf")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };

            var controller = new RegistrationController(mockService.Object);

            // Act
            var result = await controller.UploadFile(_testUserId, file);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("File size exceeds 5MB limit", badRequest.Value);
        }

        [Fact]
        public async Task UploadFile_ReturnsServerError_WhenUploadFails()
        {
            // Arrange
            var mockService = new Mock<IRegService>();
            var file = CreateFakePdfFile();
            mockService.Setup(service => service.UploadFileAsync(
                It.IsAny<Guid>(),
                It.IsAny<byte[]>(),
                It.IsAny<string>(),
                It.IsAny<long>()))
                .ThrowsAsync(new Exception("Simulated failure"));

            var controller = new RegistrationController(mockService.Object);

            // Act
            var result = await controller.UploadFile(_testUserId, file);

            // Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
            Assert.Equal("File upload failed.", serverError.Value);
        }

        // ===== Download Tests =====
        [Fact]
        public async Task DownloadFile_ReturnsFileResult_WhenFileExists()
        {
            // Arrange
            var mockService = new Mock<IRegService>();
            var fileBytes = Encoding.UTF8.GetBytes("Fake PDF file");
            mockService.Setup(service => service.GetFileAsync(_testUserId)).ReturnsAsync(fileBytes);

            var controller = new RegistrationController(mockService.Object);

            // Act
            var result = await controller.DownloadFile(_testUserId);

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("application/pdf", fileResult.ContentType);
            Assert.Equal(fileBytes, fileResult.FileContents);
        }

        [Fact]
        public async Task DownloadFile_ReturnsNotFound_WhenFileDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IRegService>();
            mockService.Setup(service => service.GetFileAsync(_testUserId)).ReturnsAsync((byte[]?)null);

            var controller = new RegistrationController(mockService.Object);

            // Act
            var result = await controller.DownloadFile(_testUserId);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No file found", notFound.Value);
        }

        // ===== Delete Tests =====
        [Fact]
        public async Task DeleteFile_ReturnsOk_WhenDeletionSuccessful()
        {
            // Arrange
            var mockService = new Mock<IRegService>();
            mockService.Setup(service => service.DeleteFileAsync(_testUserId)).ReturnsAsync(true);

            var controller = new RegistrationController(mockService.Object);

            // Act
            var result = await controller.DeleteFile(_testUserId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("File deleted successfully", okResult.Value);
        }

        [Fact]
        public async Task DeleteFile_ReturnsNotFound_WhenDeletionFails()
        {
            // Arrange
            var mockService = new Mock<IRegService>();
            mockService.Setup(service => service.DeleteFileAsync(_testUserId)).ReturnsAsync(false);

            var controller = new RegistrationController(mockService.Object);

            // Act
            var result = await controller.DeleteFile(_testUserId);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No file found/Deletion failed", notFound.Value);
        }

        // ===== Registration Status Tests =====
        [Fact]
        public async Task GetRegistrationStatus_ReturnsOk_WhenRegistrationExists()
        {
            // Arrange
            var mockService = new Mock<IRegService>();
            mockService.Setup(service => service.GetFileAsync(_testUserId)).ReturnsAsync(new byte[] { 1, 2, 3 });

            var fakeRegs = new List<Registration>
            {
                new Registration
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    Status = "Received",
                    Datetime = DateTime.UtcNow,
                    UserId = _testUserId
                }
            };

            var controller = new RegistrationController(mockService.Object, fakeRegs);

            // Act
            var result = await controller.GetRegistrationStatus(_testUserId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var registration = Assert.IsType<RegistrationDTO>(okResult.Value);
            Assert.Equal("Received", registration.Status);
        }

        [Fact]
        public async Task GetRegistrationStatus_ReturnsNotFound_WhenRegistrationMissing()
        {
            // Arrange
            var mockService = new Mock<IRegService>();
            mockService.Setup(service => service.GetFileAsync(_testUserId)).ReturnsAsync((byte[]?)null);

            var controller = new RegistrationController(mockService.Object);

            // Act
            var result = await controller.GetRegistrationStatus(_testUserId);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No registration found for this user.", notFound.Value);
        }

        // ===== Update Registration Status Tests =====
        [Fact]
        public async Task UpdateRegistrationStatus_ReturnsOk_WhenStatusUpdated()
        {
            // Arrange
            var testUserId = Guid.NewGuid();
            var mockService = new Mock<IRegService>();

            var validRegistration = new Registration
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Status = "Pending",
                Datetime = DateTime.Now,
                UserId = testUserId
            };

            var fakeRegs = new List<Registration> { validRegistration };
            var controller = new RegistrationController(mockService.Object, fakeRegs);

            // Act
            var result = controller.UpdateRegistrationStatus(new Registration { UserId = testUserId});

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedRegistration = Assert.IsType<Registration>(okResult.Value);
            Assert.Equal("Received", updatedRegistration.Status);
        }

        [Fact]
        public async Task UpdateRegistrationStatus_ReturnsBadRequest_WhenInvalidStatus()
        {
            // Arrange
            var testUserId = Guid.NewGuid();
            var mockService = new Mock<IRegService>();

            var invalidRegistration = new Registration
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Status = "InvalidStatus",
                Datetime = DateTime.Now,
                UserId = testUserId
            };

            var fakeRegs = new List<Registration> { invalidRegistration };
            var controller = new RegistrationController(mockService.Object, fakeRegs);

            // Act
            var result = controller.UpdateRegistrationStatus(new Registration { UserId = testUserId});

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("only be changed from", badRequestResult.Value.ToString());
        }
    }
}