using System.Numerics;
using WorkoutCatalogService.Shared.Constants;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.MessageBrocker.MessageBrokerService;

namespace WorkoutCatalogService.Shared.InitializerService
{
    public class DbInitializer: IDbInitializer
    {
      

        private readonly IMessageBrokerPublisher _messageBrokerPublisher;

        public DbInitializer(IMessageBrokerPublisher messageBrokerPublisher)
        {
            _messageBrokerPublisher = messageBrokerPublisher;
        }

        public async Task InitializeRabbitMQAsync() 
        {


            await _messageBrokerPublisher.creatExchange(RabbitMQConstants.PlanCreatedExchangeName);

            await _messageBrokerPublisher.creatQueue(RabbitMQConstants.WorkoutService_Plan_created_Queue);

            await _messageBrokerPublisher.BindQueue(
                RabbitMQConstants.WorkoutService_Plan_created_Queue,
                RabbitMQConstants.PlanCreatedExchangeName,
                RabbitMQConstants.PlanCreatedRoutuigKey);
        }
            
    }
}
