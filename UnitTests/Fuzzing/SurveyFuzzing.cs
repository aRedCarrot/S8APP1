using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Fuzzing
{
    public class SurveyFuzzing
    {
        private const string BEARER = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6ImFkbWluIiwibmJmIjoxNjYzNjAwNTAwLCJleHAiOjE2NjM2ODY5MDAsImlhdCI6MTY2MzYwMDUwMH0.TG7i7cl8iARb3i286QmluzYXZOeo70WE4QscqybV3ZQ";

        [Fact]
        public async Task POST_submitSurvey_OK()
        {
            var application = new WebApplicationFactory<Program>();

            var client = application.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BEARER);
            var contentBody = "{\r\n  \"surveyId\": 1,\r\n  \"responses\": [\r\n    {\r\n        \"questionId\": 1,\r\n        \"answer\": \"a\"\r\n    },\r\n    {\r\n        \"questionId\": 2,\r\n        \"answer\": \"b\"\r\n    },\r\n    {\r\n        \"questionId\": 3,\r\n        \"answer\": \"c\"\r\n    },\r\n    {\r\n        \"questionId\": 4,\r\n        \"answer\": \"d\"\r\n    }\r\n  ]\r\n}";

            var content = new StringContent(contentBody, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Survey/Submit", content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task POST_submitSurvey_Unauthorized()
        {
            var application = new WebApplicationFactory<Program>();

            var client = application.CreateClient();
            var contentBody = "{\r\n  \"surveyId\": 1,\r\n  \"responses\": [\r\n    {\r\n        \"questionId\": 1,\r\n        \"answer\": \"a\"\r\n    },\r\n    {\r\n        \"questionId\": 2,\r\n        \"answer\": \"b\"\r\n    },\r\n    {\r\n        \"questionId\": 3,\r\n        \"answer\": \"c\"\r\n    },\r\n    {\r\n        \"questionId\": 4,\r\n        \"answer\": \"d\"\r\n    }\r\n  ]\r\n}";

            var content = new StringContent(contentBody, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Survey/Submit", content);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData("{\r\n  \"surveyId\": 5,\r\n  \"responses\": [\r\n    {\r\n        \"questionId\": 1,\r\n        \"answer\": \"a\"\r\n    },\r\n    {\r\n        \"questionId\": 2,\r\n        \"answer\": \"b\"\r\n    },\r\n    {\r\n        \"questionId\": 3,\r\n        \"answer\": \"c\"\r\n    },\r\n    {\r\n        \"questionId\": 4,\r\n        \"answer\": \"d\"\r\n    }\r\n  ]\r\n}")]
        [InlineData("{\r\n  \"surveyId\": 1,\r\n  \"responses\": [\r\n    {\r\n        \"questionId\": 1,\r\n        \"answer\": \"a\"\r\n    },\r\n    {\r\n        \"questionId\": 1,\r\n        \"answer\": \"b\"\r\n    },\r\n    {\r\n        \"questionId\": 3,\r\n        \"answer\": \"c\"\r\n    },\r\n    {\r\n        \"questionId\": 4,\r\n        \"answer\": \"d\"\r\n    }\r\n  ]\r\n}")]
        [InlineData("{\r\n  \"surveyId\": 1,\r\n  \"responses\": [\r\n    {\r\n        \"questionId\": 1,\r\n        \"answer\": \"p\"\r\n    },\r\n    {\r\n        \"questionId\": 2,\r\n        \"answer\": \"b\"\r\n    },\r\n    {\r\n        \"questionId\": 3,\r\n        \"answer\": \"c\"\r\n    },\r\n    {\r\n        \"questionId\": 4,\r\n        \"answer\": \"d\"\r\n    }\r\n  ]\r\n}")]
        [InlineData("{\r\n  \"surveyId\": 1,\r\n  \"responses\": [\r\n    {\r\n        \"questionId\": 5,\r\n        \"answer\": \"a\"\r\n    },\r\n    {\r\n        \"questionId\": 2,\r\n        \"answer\": \"b\"\r\n    },\r\n    {\r\n        \"questionId\": 3,\r\n        \"answer\": \"c\"\r\n    },\r\n    {\r\n        \"questionId\": 4,\r\n        \"answer\": \"d\"\r\n    }\r\n  ]\r\n}")]
        [InlineData("{\r\n  \"surveyId\": 1,\r\n  \"responses\": [\r\n  ]\r\n}")]
        [InlineData("{\r\n  \"surveyId\": true,\r\n  \"responses\": [\r\n    {\r\n        \"questionId\": 1,\r\n        \"answer\": \"a\"\r\n    },\r\n    {\r\n        \"questionId\": 2,\r\n        \"answer\": \"b\"\r\n    },\r\n    {\r\n        \"questionId\": 3,\r\n        \"answer\": \"c\"\r\n    },\r\n    {\r\n        \"questionId\": 4,\r\n        \"answer\": \"d\"\r\n    }\r\n  ]\r\n}")]
        [InlineData("{\r\n  \"surveyId\": 1,\r\n  \"responses\": [\r\n    {\r\n        \"questionId\": true,\r\n        \"answer\": \"a\"\r\n    },\r\n    {\r\n        \"questionId\": 2,\r\n        \"answer\": \"b\"\r\n    },\r\n    {\r\n        \"questionId\": 3,\r\n        \"answer\": \"c\"\r\n    },\r\n    {\r\n        \"questionId\": 4,\r\n        \"answer\": \"d\"\r\n    }\r\n  ]\r\n}")]
        [InlineData("{\r\n  \"surveyId\": 1,\r\n  \"responses\": [\r\n    {\r\n        \"questionId\": 1,\r\n        \"answer\": \"a\"\r\n    },\r\n    {\r\n        \"questionId\": \"a\",\r\n        \"answer\": \"b\"\r\n    },\r\n    {\r\n        \"questionId\": 3,\r\n        \"answer\": \"c\"\r\n    },\r\n    {\r\n        \"questionId\": 4,\r\n        \"answer\": \"d\"\r\n    }\r\n  ]\r\n}")]
        public async Task POST_submitSurvey_BadRequest(string contentBody)
        {
            var application = new WebApplicationFactory<Program>();

            var client = application.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BEARER);
            
            var content = new StringContent(contentBody, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Survey/Submit", content);
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
        public async Task POST_submitSurvey_BadMedia(string mediaType)
        {
            var application = new WebApplicationFactory<Program>();

            var client = application.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BEARER);
            var contentBody = "{\r\n  \"surveyId\": 1,\r\n  \"responses\": [\r\n    {\r\n        \"questionId\": 1,\r\n        \"answer\": \"a\"\r\n    },\r\n    {\r\n        \"questionId\": 2,\r\n        \"answer\": \"b\"\r\n    },\r\n    {\r\n        \"questionId\": 3,\r\n        \"answer\": \"c\"\r\n    },\r\n    {\r\n        \"questionId\": 4,\r\n        \"answer\": \"d\"\r\n    }\r\n  ]\r\n}";

            var content = new StringContent(contentBody, Encoding.UTF8, mediaType);
            var response = await client.PostAsync("/Survey/Submit", content);
            response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
        }
    }
}
