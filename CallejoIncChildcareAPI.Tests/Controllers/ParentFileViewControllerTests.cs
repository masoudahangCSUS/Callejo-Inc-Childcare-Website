//using CallejoIncChildcareAPI.Controllers;
//using Common.Models.Data;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.InMemory;
//using System.Collections.Generic;
//using System.Linq;
//using Xunit;

//namespace CallejoIncChildcareAPI.Tests
//{
//    public class ParentDocumentsControllerTests
//    {
//        private readonly ParentDocumentsController _controller;

//        private readonly CallejoSystemDbContext _context;

//        public ParentDocumentsControllerTests()
//        {
//            var options = new DbContextOptionsBuilder<CallejoSystemDbContext>()
//                .UseInMemoryDatabase(databaseName: "TestDatabase")
//                .Options;
//            _context = new CallejoSystemDbContext(options);
//            _controller = new ParentDocumentsController(_context);

//            // Seed the database with test data
//            SeedDatabase();
//        }

//        private void SeedDatabase()
//        {
//            var files = new List<FileUpload>
//            {
//                new FileUpload { Id = 1, FileName = "file1.pdf", ContentType = "application/pdf", FileData = new byte[0] },
//                new FileUpload { Id = 2, FileName = "file2.pdf", ContentType = "application/pdf", FileData = new byte[0] }
//            };

//            _context.FileUploads.AddRange(files);
//            _context.SaveChanges();
//        }

//        [Fact]
//        public void GetDocuments_ReturnsOkResult_WithListOfFiles()
//        {
//            // Act
//            var result = _controller.GetDocuments();

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var returnValue = Assert.IsType<List<FileUpload>>(okResult.Value);
//            Assert.Equal(2, returnValue.Count);
//        }

//        [Fact]
//        public void PreviewDocument_ReturnsNotFound_WhenFileDoesNotExist()
//        {
//            // Act
//            var result = _controller.PreviewDocument(999); // Non-existent ID

//            // Assert
//            var notFoundResult = Assert.IsType<NotFoundResult>(result);
//        }

//        [Fact]
//        public void PreviewDocument_ReturnsFileResult_WhenFileExists()
//        {
//            // Act
//            var result = _controller.PreviewDocument(1);

//            // Assert
//            var fileResult = Assert.IsType<FileContentResult>(result);
//            Assert.Equal("application/pdf", fileResult.ContentType);
//        }

//        [Fact]
//        public void DownloadDocument_ReturnsNotFound_WhenFileDoesNotExist()
//        {
//            // Act
//            var result = _controller.DownloadDocument(999); // Non-existent ID

//            // Assert
//            var notFoundResult = Assert.IsType<NotFoundResult>(result);
//        }

//        [Fact]
//        public void DownloadDocument_ReturnsFileResult_WhenFileExists()
//        {
//            // Act
//            var result = _controller.DownloadDocument(1);

//            // Assert
//            var fileResult = Assert.IsType<FileContentResult>(result);
//            Assert.Equal("application/pdf", fileResult.ContentType);
//            Assert.Equal("file1.pdf", fileResult.FileDownloadName);
//        }
//    }
//}
