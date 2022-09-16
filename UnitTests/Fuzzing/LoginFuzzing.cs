using FluentAssertions;
using System.Net;

using Microsoft.AspNetCore.Mvc.Testing;
using CoupDeSonde;
using System.Text.Json;
using CoupDeSonde.Models;
using System.Text.Json.Nodes;
using System.Text;
using System.IO.Pipelines;
using System.Net.NetworkInformation;
using System.Text.Encodings.Web;

namespace UnitTests.Fuzzing
{
    public class LoginFuzzing
    {
        [Fact]
        public async Task POST_correctLogin()
        {
            var application = new WebApplicationFactory<Program>();

            var client = application.CreateClient();

            var content = new StringContent(getJsonUser("admin", "admin").ToString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Login", content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("{\r\n  \"username\": \"admin\",\r\n  \"password\": \"BadPassw0rd\"\r\n}")]
        [InlineData("{\r\n  \"username\": \"WhoThis\",\r\n  \"password\": \"admin\"\r\n}")]
        [InlineData("{\r\n  \"username\": \"admin\",\r\n  \r\n}")]
        [InlineData("{\r\n  \"password\": \"admin\",\r\n  \r\n}")]
        [InlineData("{\r\n  \"testUser\": \"admin\",\r\n  \"testPass\": \"admin\"\r\n}")]
        [InlineData("{\r\n  \"username\": \"admin\",\r\n  \"password\": true\r\n}")]
        [InlineData("{\r\n  \"username\": 25,\r\n  \"password\": \"admin\"\r\n}")]
        [InlineData("{}")]
        public async Task POST_badRequest(string contentString)
        {
            var application = new WebApplicationFactory<Program>();

            var client = application.CreateClient();

            var content = new StringContent(contentString, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Login", content);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        
        [Theory]
        [InlineData("application/javascript")]
        [InlineData("application/msword")]
        [InlineData("application/pdf")]
        [InlineData("application/sql")]
        [InlineData("application/vnd.ms-excel")]
        [InlineData("application/vnd.ms-powerpoint")]
        [InlineData("application/vnd.oasis.opendocument.text")]
        [InlineData("application/vnd.openxmlformats-officedocument.presentationml.presentation")]
        [InlineData("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
        [InlineData("application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
        [InlineData("application/x-www-form-urlencoded")]
        [InlineData("application/xml")]
        [InlineData("application/zip")]
        [InlineData("application/zstd")]
        [InlineData("audio/mpeg")]
        [InlineData("audio/ogg")]
        [InlineData("image/avif")]
        [InlineData("image/jpeg")]
        [InlineData("image/png")]
        [InlineData("image/svg+xml")]
        [InlineData("text/css")]
        [InlineData("text/csv")]
        [InlineData("text/html")]
        [InlineData("text/xml")]
    
        public async Task POST_badMediaType(string mediaType)
        {
            var application = new WebApplicationFactory<Program>();

            var client = application.CreateClient();


            var content = new StringContent(getJsonUser("admin", "admin").ToString(), Encoding.UTF8, mediaType);
            var response = await client.PostAsync("/Login", content);
            response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
        }

        private static string getJsonUser(string username, string password)
        {
            var json = new JsonObject();
            json.Add("username", username);
            json.Add("password", password);
            return json.ToString();
        }
    }
}
