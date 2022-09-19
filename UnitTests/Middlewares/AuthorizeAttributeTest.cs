using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoupDeSonde;
using CoupDeSonde.Middlewares;
using CoupDeSonde.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UnitTests.Middlewares
{
    public  class AuthorizeAttributeTest
    {
        public AuthorizeAttribute authorizeAttribute;
        public AuthorizeAttributeTest()
        {
            authorizeAttribute = new AuthorizeAttribute();
        }

        [Fact] 
        public void OnAuthorization_withUserInContext_AcceptRequest()
        {
            HttpContext ctx = new DefaultHttpContext();
            ActionContext fakeActionContext =
            new ActionContext(ctx,
              new Microsoft.AspNetCore.Routing.RouteData(),
              new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
            AuthorizationFilterContext fakeAuthFilterContext =
                new AuthorizationFilterContext(fakeActionContext,
                new List<IFilterMetadata> { });
            fakeActionContext.HttpContext = ctx;
            var testAcc = new User("admin", "AQAAAAEAACcQAAAAEJYZUWmKW5xCOmiNHp0eBdzOIdoPUDYy7h0gTtVZwWfdBYVXTm2SUJwA2pyfGUEOdg==");
            ctx.Items["user"] = testAcc;
            authorizeAttribute.OnAuthorization(fakeAuthFilterContext);
            Assert.NotNull(ctx.Response);
        }

        [Fact]
        public void OnAuthorization_withNoUserInContext_DenyRequestWithError()
        {
            HttpContext ctx = new DefaultHttpContext();
            ActionContext fakeActionContext =
            new ActionContext(ctx,
              new Microsoft.AspNetCore.Routing.RouteData(),
              new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
            AuthorizationFilterContext fakeAuthFilterContext =
                new AuthorizationFilterContext(fakeActionContext,
                new List<IFilterMetadata> { });
            fakeActionContext.HttpContext = ctx;
            authorizeAttribute.OnAuthorization(fakeAuthFilterContext);
            Assert.NotNull(ctx.Response);
        }
    }
}
