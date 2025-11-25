using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UserProfileService.Feature.UserProfiles.DTOs;
using UserProfileService.Shared.Entites;
using UserProfileService.Shared.GenericRepos;
using UserProfileService.Shared.Response;

namespace UserProfileService.Feature.UserProfiles.CQRS.Quries
{
    public record GetUsersByidQuery(IEnumerable<Guid> Ids) :IRequest<RequestResponse<IEnumerable<UserToReturnDto>>>;

    public class GetUsersByidQueryHandler : IRequestHandler<GetUsersByidQuery, RequestResponse<IEnumerable<UserToReturnDto>>>
    {
        private readonly IGenericRepository<UserProfile> genericRepository;

        public GetUsersByidQueryHandler(IGenericRepository<UserProfile> genericRepository)
        {
            this.genericRepository = genericRepository;
        }
        public async Task<RequestResponse<IEnumerable<UserToReturnDto>>> Handle(GetUsersByidQuery request, CancellationToken cancellationToken)
        {
            if (request.Ids == null || !request.Ids.Any())
                return RequestResponse<IEnumerable<UserToReturnDto>>.Fail("IDs cannot be null or empty", 400);

            var distinctIds = request.Ids.Distinct().Take(100).ToList();


              var userDtos = await genericRepository.GetAll()
                .Where(u => distinctIds.Contains(u.Id)).Select(u => new UserToReturnDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    DateOfBirth = u.DateOfBirth,
                    FitnessGoal = u.FitnessGoal.ToString(),
                    Gender = u.Gender.ToString(),
                    Height = u.Height,
                    planid = u.planid,
                    ProfilePictureUrl = u.ProfilePictureUrl,
                    Weight = u.Weight
                })
        .AsNoTracking()
        .ToListAsync(cancellationToken);



            return RequestResponse<IEnumerable<UserToReturnDto>>.Success(userDtos);

        }
    }
}
