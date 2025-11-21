
using Microsoft.Identity.Client;
using RabbitMQ.Client;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WorkoutCatalogService.Shared.MessageBrocker.MessageBrokerService
{
    public class MessageBrokerPublisher : IMessageBrokerPublisher
    {
        IConnection _connection;
        IChannel _channel;
        private readonly ILogger<MessageBrokerPublisher> logger;
        private readonly ConnectionFactory _factory;


        public MessageBrokerPublisher(ILogger<MessageBrokerPublisher> logger)

        {
            this.logger = logger;


            _factory = new ConnectionFactory() { 
                HostName = "localhost",
                Port= 5672,
                UserName = "admin",
                Password = "admin123",
                VirtualHost = "/"
            };

            try
            {
                _connection = _factory.CreateConnectionAsync().Result;
                logger.LogInformation("Connected to RabbitMQ");
            }
            catch (Exception ex)
            {
                logger.LogError(" RabbitMQ connection error",ex);
            }



            _channel = _connection.CreateChannelAsync().Result;

        }

        public async Task PublishMessage(string exchangeName, string routingKey, string message)
        {
            if (!string.IsNullOrEmpty(exchangeName) && !string.IsNullOrEmpty(message))
            {
                byte[] messageBody = System.Text.Encoding.UTF8.GetBytes(message);
                await _channel.BasicPublishAsync(exchangeName, routingKey, messageBody);
            }
        }

        public async Task BindQueue(string queueName, string exchangeName, string? routingKey = "")
        {
            if (!string.IsNullOrEmpty(queueName) && !string.IsNullOrEmpty(exchangeName))
            {
                await _channel.QueueBindAsync(queueName, exchangeName, routingKey ?? "");
            }

        }

        public async Task creatExchange(string exchangeName, string exchangeType = "direct")
        {
            if (!string.IsNullOrEmpty(exchangeName))
            {
                await _channel.ExchangeDeclareAsync(exchangeName, exchangeType, durable: true);

            }
        }

        public async Task creatQueue(string QueueName)
        {
            await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false);


        }
    }
}
