using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLUTUTORS.Controllers;
using VLUTUTORS.Models;
using Xunit;

namespace VLUTUTORS.Tests.ControllerTests
{
    public class HomeControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private CP25Team01Context _db = new CP25Team01Context();
        private readonly ILogger<HomeControllerTests> _logger;
        private IHostingEnvironment _environment;

        private readonly WebApplicationFactory<Startup> _factory;

        public HomeControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _logger = A.Fake<ILogger<HomeControllerTests>>();
            this._environment = A.Fake<IHostingEnvironment>();
        }
        [Theory]
        [InlineData("/")]
        [InlineData("/Home")]
        [InlineData("/Home/AboutUs")]
        [InlineData("/Home/Contact")]
        [InlineData("/Home/RegisterAsTutor")]

        public async Task GetHttpRequest(string url)
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response = await client.GetAsync(url);
            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}
