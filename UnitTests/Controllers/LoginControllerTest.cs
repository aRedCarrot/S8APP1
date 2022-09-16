using CoupDeSonde.Controllers;
using Xunit;
using Moq;
using CoupDeSonde.Services;
using Microsoft.Extensions.Logging;
using CoupDeSonde.Models;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace UnitTests.Controllers
{
    public class LoginControllerTest
    {
        public LoginController loginController;
        public Mock<IAuthentificationService> mockAuthentificationService;

        public LoginControllerTest()
        {
            mockAuthentificationService = new Mock<IAuthentificationService>();
            loginController = new LoginController(mockAuthentificationService.Object);
        }

        [Fact]
        public void Login_OnValidRequest_ReturnsOk()
        {
            var request = new LoginRequest();
            request.Username = "testRequest";
            request.Password = "testRequest";
            var expectedResponse = new LoginResponse(new User("testRequest", "testRequest"), "token");
            mockAuthentificationService.Setup<LoginResponse>(_ => _.Login(request)).Returns(expectedResponse);

            var actual = loginController.Login(request);
            actual.Should().BeEquivalentTo(new OkObjectResult(expectedResponse));
        }

        [Fact]
        public void Login_OnInvalidRequest_ReturnsBadRequest()
        {
            var request = new LoginRequest();
            request.Username = "testRequest";
            request.Password = "WrongTest";

            var actual = loginController.Login(request);
            actual.Should().BeEquivalentTo(new BadRequestObjectResult(new { message = "Username or password is incorrect" }));
        }
    }
}