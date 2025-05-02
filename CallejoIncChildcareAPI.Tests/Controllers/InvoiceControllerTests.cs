using System;
using System.Collections.Generic;
using CallejoIncChildcareAPI.Controllers;
using Common.Services.Invoice;
using Common.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace CallejoIncChildcareAPI.Tests
{
    public class InvoiceControllerTests
    {
        private readonly Mock<IInvoiceService> _mockInvoiceService;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly InvoicesController _controller;

        public InvoiceControllerTests()
        {
            _mockInvoiceService = new Mock<IInvoiceService>();
            _mockConfiguration = new Mock<IConfiguration>();
            _controller = new InvoicesController(_mockInvoiceService.Object, _mockConfiguration.Object, null);
        }

        [Fact]
        public void GetAllInvoices_ReturnsOk_WithList()
        {
            var expected = new List<InvoiceDTO> { new InvoiceDTO { InvoiceId = Guid.NewGuid(), GuardianName = "Test" } };
            _mockInvoiceService.Setup(x => x.GetAllInvoices()).Returns(expected);

            var result = _controller.GetAllInvoices();

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var list = Assert.IsType<List<InvoiceDTO>>(ok.Value);
            Assert.Single(list);
        }

        [Fact]
        public void SaveInvoice_ReturnsOk_WhenSuccess()
        {
            var dto = new InvoiceDTO { GuardianName = "Test" };
            var response = new APIResponse { Success = true, Message = "Saved" };
            _mockInvoiceService.Setup(x => x.InsertInvoice(dto)).Returns(response);

            var result = _controller.SaveInvoice(dto);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var api = Assert.IsType<APIResponse>(ok.Value);
            Assert.True(api.Success);
        }

        [Fact]
        public void UpdateInvoice_ReturnsOk_WhenSuccess()
        {
            var dto = new InvoiceDTO { GuardianName = "Updated" };
            var response = new APIResponse { Success = true, Message = "Updated" };
            _mockInvoiceService.Setup(x => x.UpdateInvoice(dto)).Returns(response);

            var result = _controller.UpdateInvoice(dto);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var api = Assert.IsType<APIResponse>(ok.Value);
            Assert.Equal("Updated", api.Message);
        }

        [Fact]
        public void DeleteInvoice_ReturnsOk_WhenSuccess()
        {
            var id = Guid.NewGuid();
            var response = new APIResponse { Success = true, Message = "Deleted" };
            _mockInvoiceService.Setup(x => x.DeleteInvoice(id)).Returns(response);

            var result = _controller.DeleteInvoice(id);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var api = Assert.IsType<APIResponse>(ok.Value);
            Assert.True(api.Success);
        }

        [Fact]
        public void GetInvoicesByGuardianId_ReturnsOk_WithList()
        {
            var id = Guid.NewGuid();
            var expected = new List<InvoiceDTO> { new InvoiceDTO { GuardianId = id, GuardianName = "Parent" } };
            _mockInvoiceService.Setup(x => x.GetInvoicesByGuardianId(id)).Returns(expected);

            var result = _controller.GetInvoicesByGuardianId(id);

            var ok = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsType<List<InvoiceDTO>>(ok.Value);
            Assert.Single(list);
        }

        [Fact]
        public void SaveInvoice_ReturnsBadRequest_WhenInsertFails()
        {
            // Arrange
            var invoice = new InvoiceDTO { GuardianId = Guid.NewGuid(), GuardianName = "Test", TotalAmount = 100 };
            _mockInvoiceService.Setup(s => s.InsertInvoice(invoice)).Returns(new APIResponse { Success = false, Message = "Insert failed." });

            // Act
            var result = _controller.SaveInvoice(invoice);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<APIResponse>(okResult.Value);
            Assert.False(response.Success);
            Assert.Equal("Insert failed.", response.Message);
        }

        [Fact]
        public void UpdateInvoice_ReturnsBadRequest_WhenInvoiceNotFound()
        {
            // Arrange
            var invoice = new InvoiceDTO { InvoiceId = Guid.NewGuid(), GuardianName = "Test" };
            _mockInvoiceService.Setup(s => s.UpdateInvoice(invoice)).Returns(new APIResponse { Success = false, Message = "Invoice not found." });

            // Act
            var result = _controller.UpdateInvoice(invoice);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<APIResponse>(okResult.Value);
            Assert.False(response.Success);
            Assert.Equal("Invoice not found.", response.Message);
        }

        [Fact]
        public void DeleteInvoice_ReturnsBadRequest_WhenInvoiceNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockInvoiceService.Setup(s => s.DeleteInvoice(id)).Returns(new APIResponse { Success = false, Message = "Invoice not found." });

            // Act
            var result = _controller.DeleteInvoice(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<APIResponse>(okResult.Value);
            Assert.False(response.Success);
            Assert.Equal("Invoice not found.", response.Message);
        }

        [Fact]
        public void GetInvoicesByGuardianId_ReturnsEmptyList_WhenNoneFound()
        {
            // Arrange
            var guardianId = Guid.NewGuid();
            _mockInvoiceService.Setup(s => s.GetInvoicesByGuardianId(guardianId)).Returns(new List<InvoiceDTO>());

            // Act
            var result = _controller.GetInvoicesByGuardianId(guardianId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var invoices = Assert.IsType<List<InvoiceDTO>>(okResult.Value);
            Assert.Empty(invoices);
        }

    }
}
