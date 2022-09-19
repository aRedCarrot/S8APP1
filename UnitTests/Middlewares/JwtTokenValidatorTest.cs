using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoupDeSonde;
using CoupDeSonde.Middlewares;
using CoupDeSonde.Models;
using CoupDeSonde.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Moq;

namespace UnitTests.Middlewares
{
    public  class JwtTokenValidatorTest
    {
        public JwtTokenValidator jwtTokenValidator;
        public HttpContext httpContext;
        public AuthentificationService authentificationService { get; set; }
        public JwtTokenValidatorTest()
        {
            var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", false)
           .Build();
            var _config = Options.Create(configuration.GetSection("S8APP1").Get<AppSettings>());
            HttpContext ctx = new DefaultHttpContext();
            RequestDelegate next = (HttpContext hc) => Task.CompletedTask;
            jwtTokenValidator = new JwtTokenValidator(next, _config);
            httpContext = new DefaultHttpContext();
            authentificationService = new AuthentificationService(_config);
        }

        [Fact]
        public async void Invoke_OnValidAuthorizationHeader_AttachUserToContext()
        {
            var testAcc = new User("admin", "AQAAAAEAACcQAAAAEJYZUWmKW5xCOmiNHp0eBdzOIdoPUDYy7h0gTtVZwWfdBYVXTm2SUJwA2pyfGUEOdg==");
            httpContext.Request.Headers.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6ImFkbWluIiwibmJmIjoxNjYzNTk4MDE3LCJleHAiOjE2NjM2ODQ0MTcsImlhdCI6MTY2MzU5ODAxN30.Ovbq4NNMt_iuKEe_-L8lwmsQ3dHveXkK88LrD--NLvc");
            await jwtTokenValidator.Invoke(httpContext, authentificationService);
            httpContext.Items["User"].Should().BeEquivalentTo(testAcc);
        }

        [Fact]
        public async void Invoke_OnInvalidAuthorizationHeader_DoNothing()
        {
            var testAcc = new User("admin", "AQAAAAEAACcQAAAAEJYZUWmKW5xCOmiNHp0eBdzOIdoPUDYy7h0gTtVZwWfdBYVXTm2SUJwA2pyfGUEOdg==");
            httpContext.Request.Headers.Add("Authorization", "Garbage Garbage");
            await jwtTokenValidator.Invoke(httpContext, authentificationService);
            httpContext.Items["User"].Should().BeNull();
        }
    }
}
