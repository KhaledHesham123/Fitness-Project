using MediatR;
using NutritionService.Shared.MessageBrocker.Messages;

namespace NutritionService.Shared.MessageBrocker.Consumers.UserTargetConsumer
{
    public class UserTargetConsumer
    {
        private readonly IMediator _mediator;

        public UserTargetConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Consume(UserTargetMessage message)
        {         
            await _mediator.Send(new SaveUserTargetCommand(

                message.UserId,
                message.DailyCalorieTarget,
                message.ProteinGoal,
                message.CarbGoal,
                message.FatGoal
            ));
        }
    }
}
