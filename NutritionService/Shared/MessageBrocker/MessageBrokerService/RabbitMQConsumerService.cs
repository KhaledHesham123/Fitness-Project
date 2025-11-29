using MediatR;
using NutritionService.Shared.MessageBrocker.Consumers;
using NutritionService.Shared.MessageBrocker.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace NutritionService.Shared.MessageBrocker.MessageBrokerService
{
    public class RabbitMQConsumerService : IHostedService
    {
        IConnection _connection;
        IChannel _channel;
        private readonly IServiceScopeFactory _scopeFactory; // Change this
        public RabbitMQConsumerService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                Port = 5672,
                UserName = "admin",
                Password = "admin123",
                VirtualHost = "/"
            };
            _connection = factory.CreateConnectionAsync().Result;
            _channel = _connection.CreateChannelAsync().Result;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _channel.ExchangeDeclareAsync(
                  exchange: "profile",
                  type: ExchangeType.Fanout,
                  durable: true
            );
            await _channel.QueueDeclareAsync(
                  queue: Constants.ProfileEventsQueue,
                  durable: true,
                  exclusive: false,
                  autoDelete: false,
                  arguments: null
             );
            await _channel.QueueBindAsync(
                 queue: Constants.ProfileEventsQueue,
                 exchange: "profile",
                 routingKey: "" // ignored for fanout
            );
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += Consumer_ReceivedAync; // push mechanism
            await _channel.BasicConsumeAsync(Constants.ProfileEventsQueue,false, consumer);
        }

        private async Task Consumer_ReceivedAync(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var message = Encoding.UTF8.GetString(@event.Body.ToArray());

                var basicMessage = GetMessage(message);

                InvokeConsumer(basicMessage);
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
            finally
            {
                //Acknowledge the message regardless of success or failure to prevent re-delivery
                await _channel.BasicAckAsync(@event.DeliveryTag,false);
            }
        }

        private async Task InvokeConsumer(BasicMessage basicMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var scopedMediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var namespaceName = "NutritionService.Shared.MessageBrocker.Consumers.UserTargetConsumer";
                var typeName = basicMessage.Type.Replace("Message", "Consumer");
                // NutritionService.Shared.MessageBrocker.Consumers.UserTargetConsumer.UserTargetConsumer
                var fullTypeName =
                    $"{namespaceName}.{typeName}, {typeof(Ref).Assembly.GetName().Name}";
                Type type = Type.GetType(fullTypeName)!;
                Console.WriteLine($"Resolving type: {type}");

                var consumer = Activator.CreateInstance(type, scopedMediator);
                var methodInfo = type.GetMethod("Consume");

                var task = (Task)methodInfo.Invoke(consumer, new object[] { basicMessage });
                task.Wait();
            }
        }

        private BasicMessage GetMessage(string message)
        {
            var basicMessage = System.Text.Json.JsonSerializer.Deserialize<BasicMessage>(message);
            var namesapce = "NutritionService.Shared.MessageBrocker.Messages";
            Type type = Type.GetType($"{namesapce}.{basicMessage?.Type},{typeof(BasicMessage).Assembly.GetName().Name}")!;
            Console.WriteLine($"Resolving type: {type}");
            return System.Text.Json.JsonSerializer.Deserialize(message, type) as BasicMessage;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
