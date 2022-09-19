using CoupDeSonde;
using CoupDeSonde.Controllers;
using CoupDeSonde.Models;
using CoupDeSonde.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Services
{
    public class AuthentificationServiceTest
    {
        public AuthentificationService authentificationService { get; set; }
        public AuthentificationServiceTest() {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .Build();
            var _config = Options.Create(configuration.GetSection("S8APP1").Get<AppSettings>());

            authentificationService = new AuthentificationService(_config);
        }

        [Fact]
        public void Login_OnValidRequest_ReturnsToken()
        {
            var request = new LoginRequest();
            request.Username = "admin";
            request.Password = "admin";

            var actual = authentificationService.Login(request);
            Assert.NotNull(actual.Token);
        }

        [Fact]
        public void Login_OnValidRequest_ReturnsNull()
        {
            var request = new LoginRequest();
            request.Username = "badRequest";
            request.Password = "badRequest";

            var actual = authentificationService.Login(request);
            Assert.Null(actual);
        }

        [Fact]
        public void GetByUsername_OnExistingUser_ReturnsValidUser()
        {
            var user = new User("admin", "AQAAAAEAACcQAAAAEJYZUWmKW5xCOmiNHp0eBdzOIdoPUDYy7h0gTtVZwWfdBYVXTm2SUJwA2pyfGUEOdg==");
            var actual = authentificationService.GetByUsername("admin");
            actual.Should().BeEquivalentTo(user);
        }
    }
}
