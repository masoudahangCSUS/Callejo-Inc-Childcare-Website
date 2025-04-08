using Xunit;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using Common.AES;
using Microsoft.AspNetCore.Mvc;

namespace CallejoIncChildCareAPI.Authentication.Tests
{
    public class AuthenticateActionTests
    {
        [Fact]
        public void OnAuthorization_ValidRequest_ShouldAllowAccess()
        {
            // Arrange
            var filterContextMock = new Mock<AuthorizationFilterContext>();
            var httpContextMock = new Mock<HttpContext>();
            var requestHeaders = new HeaderDictionary
            {
                { "AppId", AesOperation.EncryptString(AuthenticateAction.Key, $"{DateTime.Today:MM/dd/yyyy}|{Guid.NewGuid()}") }
            };

            httpContextMock.Setup(c => c.Request.Headers).Returns(requestHeaders);
            filterContextMock.Setup(c => c.HttpContext).Returns(httpContextMock.Object);

            AuthenticateAction.Applications = new List<Guid> { Guid.NewGuid() };
            AuthenticateAction.Key = "testKey";

            var action = new AuthenticateAction();

            // Act
            action.OnAuthorization(filterContextMock.Object);

            // Assert
            Assert.Null(filterContextMock.Object.Result); // Ensure no unauthorized result
        }

        [Fact]
        public void OnAuthorization_InvalidRequest_ShouldReturnUnauthorizedResult()
        {
            // Arrange
            var filterContextMock = new Mock<AuthorizationFilterContext>();
            var httpContextMock = new Mock<HttpContext>();
            var requestHeaders = new HeaderDictionary
            {
                { "AppId", "InvalidEncryptedData" }
            };

            httpContextMock.Setup(c => c.Request.Headers).Returns(requestHeaders);
            filterContextMock.Setup(c => c.HttpContext).Returns(httpContextMock.Object);

            AuthenticateAction.Applications = new List<Guid> { Guid.NewGuid() };
            AuthenticateAction.Key = "testKey";

            var action = new AuthenticateAction();

            // Act
            action.OnAuthorization(filterContextMock.Object);

            // Assert
            Assert.IsType<UnauthorizedResult>(filterContextMock.Object.Result); // Ensure unauthorized result
        }

        [Fact]
        public void OnAuthorization_ExpiredDate_ShouldReturnUnauthorizedResult()
        {
            // Arrange
            var filterContextMock = new Mock<AuthorizationFilterContext>();
            var httpContextMock = new Mock<HttpContext>();
            var expiredDate = DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy");
            var requestHeaders = new HeaderDictionary
            {
                { "AppId", AesOperation.EncryptString(AuthenticateAction.Key, $"{expiredDate}|{Guid.NewGuid()}") }
            };

            httpContextMock.Setup(c => c.Request.Headers).Returns(requestHeaders);
            filterContextMock.Setup(c => c.HttpContext).Returns(httpContextMock.Object);

            AuthenticateAction.Applications = new List<Guid> { Guid.NewGuid() };
            AuthenticateAction.Key = "testKey";

            var action = new AuthenticateAction();

            // Act
            action.OnAuthorization(filterContextMock.Object);

            // Assert
            Assert.IsType<UnauthorizedResult>(filterContextMock.Object.Result); // Ensure unauthorized result
        }
    }
}
