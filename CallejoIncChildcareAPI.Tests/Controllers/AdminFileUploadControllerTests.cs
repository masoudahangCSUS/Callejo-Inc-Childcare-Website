using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Common.Models.Data;
using System.Collections.Generic;
using CallejoIncChildcareAPI.Controllers;
using Microsoft.EntityFrameworkCore;
using Common.View;

namespace CallejoIncChildcareAPI.Tests
{
    public class AdminFileUploadControllerTests
    {
        private readonly Mock<CallejoSystemDbContext> _mockContext;
        private readonly AdminFileUploadController _controller;

        public AdminFileUploadControllerTests()
        {
            _mockContext = new Mock<CallejoSystemDbContext>();
            _controller = new AdminFileUploadController(_mockContext.Object);
        }

        // Helper method to create a mock DbSet
        private Mock<DbSet<T>> CreateMockDbSet<T>(IEnumerable<T> data) where T : class
        {
            var queryableData = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());

            return mockSet;
        }


        [Fact]
        public async Task UploadFile_ReturnsBadRequest_WhenFileIsNull()
        {
            // Arrange
            IFormFile file = null;
            string documentType = "TestDocumentType";

            // Act
            var result = await _controller.UploadFile(file, documentType);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UploadFile_ReturnsBadRequest_WhenFileExceedsSizeLimit()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(20_000_000); // 20MB
            fileMock.Setup(f => f.FileName).Returns("test.pdf");
            string documentType = "TestDocumentType";

            // Act
            var result = await _controller.UploadFile(fileMock.Object, documentType);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UploadFile_ReturnsOk_WhenValidFileIsUploaded()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write("Dummy file content");
            writer.Flush();
            ms.Position = 0;

            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
            fileMock.Setup(f => f.Length).Returns(ms.Length);
            fileMock.Setup(f => f.FileName).Returns("test.pdf");
            fileMock.Setup(f => f.ContentType).Returns("application/pdf");

            string documentType = "TestDocumentType";

            var dbSetMock = CreateMockDbSet(new List<FileUpload>()); // Reuse helper method for DbSet
            _mockContext.Setup(c => c.FileUploads).Returns(dbSetMock.Object);

            // Act
            var result = await _controller.UploadFile(fileMock.Object, documentType);

            // Assert
            Assert.NotNull(result); // Ensure result is not null
            Assert.IsAssignableFrom<OkObjectResult>(result); // Check it's an OkObjectResult or derived
        }

        [Fact]
        public void DownloadFile_ReturnsNotFound_WhenFileDoesNotExist()
        {
            // Arrange
            int documentId = 1;

            var mockFileUploads = CreateMockDbSet(new List<FileUpload>()); // Empty data set
            _mockContext.Setup(c => c.FileUploads).Returns(mockFileUploads.Object);

            // Act
            var result = _controller.DownloadFile(documentId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void DownloadFile_ReturnsFile_WhenFileExists()
        {
            // Arrange
            int documentId = 1;

            var fileData = new byte[] { 0x01, 0x02, 0x03 };
            var fileUpload = new FileUpload
            {
                Id = documentId,
                FileName = "test.pdf",
                ContentType = "application/pdf",
                FileData = fileData
            };

            var mockFileUploads = CreateMockDbSet(new List<FileUpload> { fileUpload });
            _mockContext.Setup(c => c.FileUploads).Returns(mockFileUploads.Object);

            // Act
            var result = _controller.DownloadFile(documentId) as FileContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("application/pdf", result.ContentType);
            Assert.Equal(fileData, result.FileContents);
        }

        [Fact]
        public void PreviewFile_ReturnsNotFound_WhenFileDoesNotExist()
        {
            // Arrange
            int documentId = 1;

            var mockFileUploads = CreateMockDbSet(new List<FileUpload>()); // Empty data set
            _mockContext.Setup(c => c.FileUploads).Returns(mockFileUploads.Object);

            // Act
            var result = _controller.PreviewFile(documentId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void PreviewFile_ReturnsFile_ForPDFFile()
        {
            // Arrange
            int documentId = 1;

            var fileData = new byte[] { 0x01, 0x02, 0x03 }; // Simulated PDF file data
            var fileUpload = new FileUpload
            {
                Id = documentId,
                FileName = "test.pdf",
                ContentType = "application/pdf",
                FileData = fileData
            };

            var mockFileUploads = CreateMockDbSet(new List<FileUpload> { fileUpload });
            _mockContext.Setup(c => c.FileUploads).Returns(mockFileUploads.Object);

            // Act
            var result = _controller.PreviewFile(documentId) as FileContentResult;

            // Assert
            Assert.NotNull(result); // Ensure a result is returned
            Assert.Equal("application/pdf", result.ContentType); // Validate the ContentType
            Assert.Equal(fileData, result.FileContents); // Validate the file data
        }
    }
}