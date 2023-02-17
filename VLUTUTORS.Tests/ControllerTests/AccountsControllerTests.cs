using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using VLUTUTORS.Controllers;
using VLUTUTORS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace VLUTUTORS.Tests.ControllerTests
{
    public class AccountsControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private CP25Team01Context db = new CP25Team01Context();
        private Func<Taikhoannguoidung, IActionResult> _loginSuccessCallback;
        private readonly ILogger<AccountsController> _logger;
        private IHostingEnvironment _environment;

        public AccountsControllerTests(WebApplicationFactory<Startup> factory, ILogger<AccountsController> logger, IHostingEnvironment environment)
        {
            _factory = factory;
            _logger = logger;
            this._environment = environment;
        }
        [Theory]
        [InlineData("/")]
        [InlineData("/Accounts/Login")]
        [InlineData("/Accounts/VerifyAccount")]
        [InlineData("/Accounts/ForGotPass")]
        [InlineData("/Accounts/Details/1")]
        [InlineData("/Accounts/EditLearnerAccounts/1")]

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
