using MediatR;
using UserProfileService.Contract;
using UserProfileService.Entites;
using UserProfileService.Shared.Dto;
using UserProfileService.Shared.Response;

namespace UserProfileService.Features.Commend
{
    public record AddUsreProgress(Guid id , decimal CurrentWeight, DateOnly MeasurementDate) : IRequest<RequestResponse<USerProgressDTo>>;
    public class AddUsreProgressHandler(IGenericRepository<UserProgress> repository) : IRequestHandler<AddUsreProgress, RequestResponse<USerProgressDTo>>
    {
        public async Task<RequestResponse<USerProgressDTo>> Handle(AddUsreProgress request, CancellationToken cancellationToken)
        {
            if (request.id == Guid.Empty)
                return RequestResponse<USerProgressDTo>.Fail("error there is not Id", 404);
            try
            {
                var model = new UserProgress
                {
                    Id = request.id,
                    CurrentWeight = request.CurrentWeight,
                    MeasurementDate = request.MeasurementDate
                };
                repository.Add(model);
                await repository.SaveChangesAsync();
                
            }
            catch (Exception ex) {
                return RequestResponse<USerProgressDTo>.Fail(ex.ToString(), 400);
            }
            var progress = new USerProgressDTo
            {
                CurrentWeight = request.CurrentWeight,
                MeasurementDate = request.MeasurementDate
            };
           
            return  RequestResponse<USerProgressDTo>.Success(progress, "plan add Progress successfily");
        }
    }
}
