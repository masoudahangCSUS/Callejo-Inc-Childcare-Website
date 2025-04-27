using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using CallejoIncChildcareAPI.Controllers;
using Common.Services.Expenses;
using Common.View;

namespace CallejoIncChildcareAPI.Tests
{
    public class ExpenseControllerTests
    {
        // Fake PDF file setup
        private IFormFile CreateFakePdfFile()
        {
            var content = "Fake File";
            var fileName = "test.pdf";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            return new FormFile(stream, 0, stream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };
        }
        // GetAllExpenses
        [Fact]
        public async Task GetAllExpenses_ReturnsOkResults_WhenSuccess()
        {
            // Arrange
            var mockService = new Mock<IExpenseService>();
            mockService.Setup(service => service.GetAllExpensesAsync())
                .ReturnsAsync(new List<ExpenseDTO> { new ExpenseDTO { Id = 1, Amount = 50 } });

            var controller = new ExpenseController(mockService.Object);

            // Act
            var result = await controller.GetAllExpenses();

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<ExpenseDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var data = Assert.IsType<List<ExpenseDTO>>(okResult.Value);
            Assert.Single(data);
            Assert.Equal(1, data[0].Id);
            Assert.Equal(50, data[0].Amount);
        }

        [Fact]
        public async Task GetAllExpenses_ReturnsBadRequest_WhenNull()
        {
            // Arrange
            var mockService = new Mock<IExpenseService>();
            mockService.Setup(service => service.GetAllExpensesAsync())
                .ReturnsAsync((List<ExpenseDTO>)null);

            var controller = new ExpenseController(mockService.Object);

            // Act
            var result = await controller.GetAllExpenses();

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<ExpenseDTO>>>(result);
            Assert.IsType<BadRequestResult>(actionResult.Result);
        }


        // GetTotalExpenses
        [Fact]
        public async Task GetTotalExpenses_ReturnsOkResult_WhenSuccess()
        {
            // Arrange
            var mockService = new Mock<IExpenseService>();
            mockService.Setup(service => service.GetTotalExpensesAsync())
                .ReturnsAsync(1234.56m);

            var controller = new ExpenseController(mockService.Object);

            // Act
            var result = await controller.GetTotalExpenses();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var total = Assert.IsType<decimal>(okResult.Value);

            Assert.Equal(1234.56m, total);
        }

        [Fact]
        public async Task GetTotalExpenses_ReturnsBadRequest_WhenFailure()
        {
            // Arrange
            var mockService = new Mock<IExpenseService>();
            mockService.Setup(service => service.GetTotalExpensesAsync())
                .ReturnsAsync((decimal)0);

            var controller = new ExpenseController(mockService.Object);

            // Act
            var result = await controller.GetTotalExpenses();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var total = Assert.IsType<decimal>(okResult.Value);
            Assert.Equal(0m, total);
        }

        // GetChildrenCount
        [Fact]
        public async Task GetChildrenCount_ReturnsOkResult_WhenSuccess()
        {
            // Arrange
            var mockService = new Mock<IExpenseService>();
            mockService.Setup(service => service.GetChildrenCountAsync())
                .ReturnsAsync(5);

            var controller = new ExpenseController(mockService.Object);

            // Act
            var result = await controller.GetChildrenCount();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var count = Assert.IsType<int>(okResult.Value);

            Assert.Equal(5, count);
        }

        [Fact]
        public async Task GetChildrenCount_ReturnsBadRequest_WhenZero()
        {
            // Arrange
            var mockService = new Mock<IExpenseService>();
            mockService.Setup(service => service.GetChildrenCountAsync())
                .ReturnsAsync(-1);

            var controller = new ExpenseController(mockService.Object);

            // Act
            var result = await controller.GetChildrenCount();

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        // CreateExpense
        [Fact]
        public async Task CreateExpense_ReturnsOkResult_WhenSuccess()
        {
            // Arrange
            var newExpense = new ExpenseDTO { Id = 1, Amount = 99.99m };
            var mockService = new Mock<IExpenseService>();
            mockService.Setup(service => service.CreateExpenseAsync(It.IsAny<ExpenseDTO>()))
                .ReturnsAsync(newExpense);

            var controller = new ExpenseController(mockService.Object);

            var file = CreateFakePdfFile();
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string note = "Upload Test Success";
            decimal amount = 20.0m;
            string category = "Supplies";

            // Act
            var result = await controller.UploadExpense(file, category, date, amount, note);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ExpenseDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var expense = Assert.IsType<ExpenseDTO>(okResult.Value);

            Assert.Equal(1, expense.Id);
            Assert.Equal(99.99m, expense.Amount);
        }

        [Fact]
        public async Task CreateExpense_ReturnsBadRequest_WhenFailure()
        {
            // Arrange
            var mockService = new Mock<IExpenseService>();
            mockService.Setup(service => service.CreateExpenseAsync(It.IsAny<ExpenseDTO>()))
                .ReturnsAsync((ExpenseDTO)null);

            var controller = new ExpenseController(mockService.Object);

            var file = CreateFakePdfFile();
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string note = "Upload Test Fail";
            decimal amount = 20.0m;
            string category = "Supplies";

            // Act
            var result = await controller.UploadExpense(file, category, date, amount, note);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ExpenseDTO>>(result);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        // EditExpense
        [Fact]
        public async Task EditExpense_ReturnsOkResult_WhenSuccess()
        {
            // Arrange
            var mockService = new Mock<IExpenseService>();
            mockService.Setup(service => service.EditExpenseAsync(It.IsAny<ExpenseDTO>()))
                .ReturnsAsync(true);

            var controller = new ExpenseController(mockService.Object);

            int id = 1;
            IFormFile file = null;
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string note = "Edit Test Success";
            decimal amount = 20.0m;
            string category = "Supplies";

            // Act
            var result = await controller.EditExpense(id, file, category, date, amount, note);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var success = Assert.IsType<bool>(okResult.Value);

            Assert.True(success);
        }

        [Fact]
        public async Task EditExpense_ReturnsBadRequest_WhenFailure()
        {
            // Arrange
            var mockService = new Mock<IExpenseService>();
            mockService.Setup(service => service.EditExpenseAsync(It.IsAny<ExpenseDTO>()))
                .ReturnsAsync(false);

            var controller = new ExpenseController(mockService.Object);

            int id = 1;
            IFormFile file = null;
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string note = "Edit Test Fail";
            decimal amount = 20.0m;
            string category = "Supplies";

            // Act
            var result = await controller.EditExpense(id, file, category, date, amount, note);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        // DeleteExpense
        [Fact]
        public async Task DeleteExpense_ReturnsOkResult_WhenSuccess()
        {
            // Arrange
            var mockService = new Mock<IExpenseService>();
            mockService.Setup(service => service.DeleteExpenseAsync(1))
                .ReturnsAsync(true);

            var controller = new ExpenseController(mockService.Object);

            // Act
            var result = await controller.DeleteExpense(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var success = Assert.IsType<bool>(okResult.Value);

            Assert.True(success);
        }

        [Fact]
        public async Task DeleteExpense_ReturnsBadRequest_WhenFailure()
        {
            // Arrange
            var mockService = new Mock<IExpenseService>();
            mockService.Setup(service => service.DeleteExpenseAsync(1))
                .ReturnsAsync(false);

            var controller = new ExpenseController(mockService.Object);

            // Act
            var result = await controller.DeleteExpense(1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        // DownloadReceipt
        [Fact]
        public async Task DownloadReceipt_ReturnsFileResult_WhenReceiptExists()
        {
            // Arrange
            var mockService = new Mock<IExpenseService>();
            byte[] fakeReceipt = new byte[] { 1, 2, 3 };
            mockService.Setup(service => service.DownloadExpenseAsync(It.IsAny<int>()))
                .ReturnsAsync(fakeReceipt);

            var controller = new ExpenseController(mockService.Object);

            // Act
            var result = await controller.DownloadFile(1);

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("application/pdf", fileResult.ContentType);
            Assert.Equal(fakeReceipt, fileResult.FileContents);
            Assert.Equal("receipt.pdf", fileResult.FileDownloadName);
        }

        [Fact]
        public async Task DownloadReceipt_ReturnsNotFound_WhenReceiptIsNull()
        {
            // Arrange
            var mockService = new Mock<IExpenseService>();
            mockService.Setup(service => service.DownloadExpenseAsync(It.IsAny<int>()))
                       .ReturnsAsync((byte[]?)null); // Simulate missing receipt

            var controller = new ExpenseController(mockService.Object);

            // Act
            var result = await controller.DownloadFile(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("No file found", notFoundResult.Value?.ToString());
        }
    }
}
