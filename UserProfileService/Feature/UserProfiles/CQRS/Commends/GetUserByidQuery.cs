using MediatR;
using Microsoft.EntityFrameworkCore;
using UserProfileService.Feature.UserProfiles.DTOs;
using UserProfileService.Shared.Entites;
using UserProfileService.Shared.GenericRepos;
using UserProfileService.Shared.Response;

namespace UserProfileService.Feature.UserProfiles.CQRS.Commends
{
    public record GetUserByidQuery(Guid id):IRequest<RequestResponse<UserToReturnDto>>;

    public class GetUserByidQueryHandler : IRequestHandler<GetUserByidQuery, RequestResponse<UserToReturnDto>>
    {
        private readonly IGenericRepository<UserProfile> genericRepository;

        public GetUserByidQueryHandler(IGenericRepository<UserProfile> genericRepository)
        {
            this.genericRepository = genericRepository;
        }
        public async Task<RequestResponse<UserToReturnDto>> Handle(GetUserByidQuery request, CancellationToken cancellationToken)
        {
            if (request.id == Guid.Empty)
                return RequestResponse<UserToReturnDto>.Fail("ID can not be ull",400);
            var user = await genericRepository.GetByIdQueryable(request.id).FirstOrDefaultAsync();
            if (user == null)
                return RequestResponse<UserToReturnDto>.Fail("No User have this id", 400);

            var MappedUser = new UserToReturnDto
            {
                DateOfBirth = user.DateOfBirth,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FitnessGoal = user.FitnessGoal,
                Gender = user.Gender,
                Height = user.Height,
                planid = user.planid,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Weight = user.Weight,
            };

            return RequestResponse<UserToReturnDto>.Success(MappedUser);

        }
    }
}
