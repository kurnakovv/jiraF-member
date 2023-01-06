using jiraF.Member.API.Infrastructure.Data.Contexts;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Threading.Tasks;
using System.Net;
using jiraF.Member.API.GlobalVariables;

namespace jiraF.Member.EndToEndTests
{
    public class PingTests : IDisposable
    {
        private readonly HttpClient _client;

        public PingTests()
        {
            TestVariables.IsWorkNow = true;
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddDbContext<AppDbContext>(options =>
                        {
                            options.UseInMemoryDatabase(Guid.NewGuid().ToString());
                        });
                    });
                });

            _client = application.CreateClient();
        }

        public void Dispose()
        {
            TestVariables.IsWorkNow = false;
        }

        [Fact]
        public async Task Ping_CanCheckHealthOfHost_Healtly()
        {
            HttpResponseMessage response = await _client.GetAsync("/ping");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Healthy", await response.Content.ReadAsStringAsync());
        }
    }
}
