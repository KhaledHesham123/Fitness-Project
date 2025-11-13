namespace UserProfileService.Shared.MessageBrocker.MessageBrokerService
{
    public interface IMessageBrokerPublisher
    {
        Task PublishMessage(string exchangeName, string routingKey, string message);

        Task BindQueue(string queueName, string exchangeName, string routingKey);

        Task creatExchange(string exchangeName, string exchangeType= "direct");
        Task creatQueue(string QueueName);


    }
}
