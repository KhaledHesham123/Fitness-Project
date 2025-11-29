//using MediatR;
//using System.Threading.Tasks;
//using UserProfileService.Entites;
//using UserProfileService.Features.Query;
//using UserProfileService.Shared.MessageBrocker.Messages;

//namespace UserProfileService.Shared.MessageBrocker.Consumers
//{
//    public class PLanCreatedConsumer
//    {
//        private readonly IMediator _mediator;

//        public PLanCreatedConsumer(IMediator mediator)
//        {
//            this._mediator = mediator;
//        }

//        public async Task Consume(BasicMessage basicMessage)
//        {
//            var msg = basicMessage as PlanAddedMessage;

//            var user = await _mediator.Send(new GetUserprofile(msg.Userid));
//            var mapedUser = new UserProfile
//            {
//                DateOfBirth = user.Data.DateOfBirth,
//                FirstName = user.Data.FirstName,
//                LastName = user.Data.LastName,
//                FitnessGoal = user.Data.FitnessGoal,
//                Gender = user.Data.Gender,
//                Height = user.Data.Height,
//                ProfilePictureUrl = user.Data.ProfilePictureUrl,
//               planid = user.Data.
//                Weight = user.Data.Weight,

//            }

//            if (user.IsSuccess)
//            {
//                await _mediator.Send(new AssginplanToUserCommend(mapedUser, msg.planid));


//            }



//        }


//    }
//}
