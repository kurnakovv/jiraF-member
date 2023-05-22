using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace jiraF.Member.API.Infrastructure.RabbitMQ;

public class RabbitMqService : IRabbitMqService
{
    private readonly IConfiguration _configuration;

    public RabbitMqService(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

	public void SendMessage(object obj)
	{
		var message = JsonSerializer.Serialize(obj);
		SendMessage(message);
	}

	public void SendMessage(string message)
	{
        string userName = _configuration["RABBITMQ_DEFAULT_USER"] ?? Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_USER");
        string password = _configuration["RABBITMQ_DEFAULT_PASS"] ?? Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_PASS");
        string hostName = _configuration.GetValue<string>("RabbitMQ:HostName");
        int port = 5671;
        string ciTests = _configuration.GetValue<string>("RabbitMQ:CITests");
        if (userName == ciTests && password == ciTests)
        {
            return;
        }
        ConnectionFactory factory = new()
        {
            Uri = new Uri($"amqps://{userName}:{password}@{hostName}/{userName}"),
            HostName = hostName,
            Port = port,
            UserName = userName,
            Password = password,
        };
        using IConnection connection = factory.CreateConnection();
        using IModel channel = connection.CreateModel();
        channel.QueueDeclare(queue: "MemberQueue",
                       durable: false,
                       exclusive: false,
                       autoDelete: false,
                       arguments: null);
        byte[] body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "",
                       routingKey: "MemberQueue",
                       basicProperties: null,
                       body: body);
    }
}

