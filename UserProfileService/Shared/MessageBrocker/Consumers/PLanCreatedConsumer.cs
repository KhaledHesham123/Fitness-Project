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

            var user = await _mediator.Send(new GetUsersByidQuery(msg.Userid));
            var mapedUser = user.Data.Select(x => new UserProfile 
            {
                Id = x.Id,
                DateOfBirth = x.DateOfBirth,
                FirstName = x.FirstName,
                LastName = x.LastName,
                FitnessGoal = Enum.Parse<FitnessGoal>(x.FitnessGoal),
                Gender = Enum.Parse<Gender>(x.Gender),                
                Height = x.Height,
                ProfilePictureUrl = x.ProfilePictureUrl,
                planid = x.planid,
                Weight = x.Weight,

            }).ToList();
            if (user.IsSuccess)
            {
                await _mediator.Send(new AssginplanToUserCommend(mapedUser, msg.planid));


            }


            
        }

        
    }
}
