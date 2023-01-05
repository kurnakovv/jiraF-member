using jiraF.Member.API.Infrastructure.RabbitMQ;
using Microsoft.Extensions.Configuration;

namespace jiraF.Member.IntegrationTests.Infrastructure.RabbitMQ
{
    public class RabbitMQServiceTests
    {
        private readonly IConfiguration _configuration;
        private readonly RabbitMqService _rabbitMqService;

        public RabbitMQServiceTests()
        {
            Dictionary<string, string> inMemorySettings = new() {
                {"RABBITMQ_DEFAULT_USER", Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_USER")},
                {"RABBITMQ_DEFAULT_PASS", Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_PASS")},
                {"RabbitMQ:HostName", "armadillo-01.rmq.cloudamqp.com"},
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            _rabbitMqService = new RabbitMqService(_configuration);
        }

        [Fact]
        public void SendMessage_CanSendMessageWithStringWithoutException_SuccessWithoutExceptions()
        {
            Exception exception = Record.Exception(() => _rabbitMqService.SendMessage("TestMessage"));
            Assert.Null(exception);
        }
    }
}
