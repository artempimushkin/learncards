using System;
using Xunit;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Web;
using Infrastructure;
using Application;
using Domain;
using Newtonsoft.Json;
using Application.DTOs;

namespace IntegrationTest
{
    public class UnitTest1 : IClassFixture<WebApplicationFactory<Web.Startup>>
    {
        private readonly WebApplicationFactory<Web.Startup> _factory;

        public UnitTest1(WebApplicationFactory<Web.Startup> factory)
        {
            _factory = factory;
        }

        //[Theory]
        //[InlineData("/api/GetDeckList")]
        //public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        //{
        //    // Arrange
        //    var client = _factory.CreateClient();

        //    // Act
        //    var response = await client.GetAsync(url);

        //    // Assert
        //    response.EnsureSuccessStatusCode(); // Status Code 200-299
        //    Assert.Equal("text/html; charset=utf-8",
        //        response.Content.Headers.ContentType.ToString());
        //}

        [Fact]
        public async Task Get_SecurePageIsReturnedForAnAuthenticatedUser()
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddAuthentication("pimushkin")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                                "pimushkin", options => { });
                    });
                })
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                });

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("pimushkin");

            //Act
            var response = await client.GetAsync("/api/GetDeckList");
            var decks = JsonConvert.DeserializeObject<List<DeckElementDto>>(response.Content.ReadAsStringAsync().Result);

            // Assert
            Assert.Equal(4, decks.Count);
        }
    }

    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[] { new Claim(ClaimTypes.Name, "pimushkin") };
            var identity = new ClaimsIdentity(claims, "pimushkin");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "pimushkin");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
