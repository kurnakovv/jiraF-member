﻿using jiraF.Member.API.GlobalVariables;
using jiraF.Member.API.Infrastructure.RabbitMQ;
using Microsoft.Extensions.Configuration;

namespace jiraF.Member.IntegrationTests.Infrastructure.RabbitMQ
{
    public class RabbitMQServiceTests : IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly RabbitMqService _rabbitMqService;

        public RabbitMQServiceTests()
        {
            TestVariables.IsWorkNow = true;
            Dictionary<string, string> inMemorySettings = new() {
                {"RABBITMQ_DEFAULT_USER", Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_USER")},
                {"RABBITMQ_DEFAULT_PASS", Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_PASS")},
                {"RabbitMQ:HostName", "armadillo-01.rmq.cloudamqp.com"},
                {"RabbitMQ:IsLocalhost", "false"}
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            _rabbitMqService = new RabbitMqService(_configuration);
        }

        public void Dispose()
        {
            TestVariables.IsWorkNow = false;
        }

        [Fact]
        public void SendMessage_CanSendMessageWithStringWithoutException_SuccessWithoutExceptions()
        {
            Exception exception = Record.Exception(() => _rabbitMqService.SendMessage("TestMessage"));
            Assert.Null(exception);
        }

        [Fact]
        public void SendMessage_CanStopWorkingWithCITestsValues_SuccessWithoutExceptions()
        {
            Dictionary<string, string> inMemorySettings = new() {
                {"RABBITMQ_DEFAULT_USER", "CI_TESTS"},
                {"RABBITMQ_DEFAULT_PASS", "CI_TESTS"},
                {"RabbitMQ:CITests", "CI_TESTS" },
            };
            var configurable = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var rabbitMqService = new RabbitMqService(configurable);
            Exception exception = Record.Exception(() => rabbitMqService.SendMessage("TestMessage"));
            Assert.Null(exception);
        }

        [Fact]
        public void SendMessage_CanSendMessageWithObjectWithoutException_SuccessWithoutExceptions()
        {
            Exception exception = Record.Exception(() => _rabbitMqService.SendMessage((object)new string("TestMessage")));
            Assert.Null(exception);
        }
    }
}
