using MediatR;

namespace UserProfileService.Shared.MessageBrocker.Consumers
{
    public class PLanCreatedConsumer
    {
        private readonly IMediator _mediator;

        public PLanCreatedConsumer(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public void Consume(BasicMessage basicMessage) 
        {
            
        }

        
    }
}
