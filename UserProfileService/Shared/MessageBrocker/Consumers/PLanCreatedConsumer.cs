using MediatR;
using System.Threading.Tasks;
using UserProfileService.Feature.UserProfiles.CQRS.Commends;
using UserProfileService.Feature.UserProfiles.CQRS.Quries;
using UserProfileService.Shared.Entites;
using UserProfileService.Shared.MessageBrocker.Messages;

namespace UserProfileService.Shared.MessageBrocker.Consumers
{
    public class PLanCreatedConsumer
    {
        private readonly IMediator _mediator;

        public PLanCreatedConsumer(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task Consume(BasicMessage basicMessage) 
        {
            var msg = basicMessage as PlanAddedMessage;

            var user = await _mediator.Send(new GetUserByidQuery(msg.Userid));
            var mapedUser = new UserProfile 
            {
                Id = user.Data.Id,
                DateOfBirth = user.Data.DateOfBirth,
                FirstName= user.Data.FirstName,
                LastName = user.Data.LastName,
                FitnessGoal = user.Data.FitnessGoal,
                Gender=user.Data.Gender,
                Height=user.Data.Height,
                ProfilePictureUrl=user.Data.ProfilePictureUrl, 
               planid=user.Data.planid,
               Weight=user.Data.Weight,
               
            };

            if (user.IsSuccess)
            {
                await _mediator.Send(new AssginplanToUserCommend(mapedUser, msg.planid));


            }


            
        }

        
    }
}
