using MediatR;
using NutritionService.Shared.MessageBrocker.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection.Metadata;
using System.Text;

namespace NutritionService.Shared.MessageBrocker.MessageBrokerService
{
    public class RabbitMQConsumerService : IHostedService
    {
        IConnection _connection;
        IChannel _channel;
        IMediator _mediator;
        public RabbitMQConsumerService(IMediator mediator)
        {
            _mediator = mediator;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnectionAsync().Result;
            _channel = _connection.CreateChannelAsync().Result;
            // _channel.BasicGetAsync() // pull mechanism
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += Consumer_ReceivedAync; // push mechanism
            await _channel.BasicConsumeAsync("Added_Products", false, consumer);
            await _channel.BasicConsumeAsync("Deleted_Products", false, consumer);
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
                await _channel.BasicAckAsync(@event.DeliveryTag, false);
            }
        }

        private void InvokeConsumer(BasicMessage basicMessage)
        {
            var namespaceName = "Services.RabbitMQ.Consumers";
            var typeName = basicMessage.Type.Replace("Message", "Consumer");

            Type type = Type.GetType($"{namespaceName}.{typeName},{typeof(AssemblyReference).Assembly.GetName().Name}");


            var consumer = Activator.CreateInstance(type, _mediator);
            var methodInfo = type.GetMethod("Consume");

            methodInfo.Invoke(consumer, new object[] { basicMessage });

        }

        private BasicMessage GetMessage(string message)
        {
            var basicMessage = System.Text.Json.JsonSerializer.Deserialize<BasicMessage>(message);
            var namesapce = "Shared.RabbitMQ";
            Type type = Type.GetType($"{namesapce}.{basicMessage?.Type},{typeof(BasicMessage).Assembly.GetName().Name}")!;

            return System.Text.Json.JsonSerializer.Deserialize(message, type) as BasicMessage;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
