using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserProfileService.Contract;
using UserProfileService.Entites;
using UserProfileService.Shared.Dto;
using UserProfileService.Shared.Response;

namespace UserProfileService.Features.Query
{
    public record GetUserProgress (Guid Guid,int PageNumber,int PageSize,decimal? SearchWeight , DateOnly? Date) :IRequest<RequestResponse<PagedResult<USerProgressDTo>>>;

    public class GetUserProgressHaddler(IGenericRepository<UserProgress> repository) : IRequestHandler<GetUserProgress, RequestResponse<PagedResult<USerProgressDTo>>>
    {
        public async Task<RequestResponse<PagedResult<USerProgressDTo>>> Handle(GetUserProgress request, CancellationToken cancellationToken)
        {
           if(request.Guid == Guid.Empty)
                return RequestResponse<PagedResult<USerProgressDTo>>.Fail("There is  no id", 400);
           var model   = await repository.FindAsync(x => x.UserId == request.Guid);
            if (model == null || !model.Any())
                return RequestResponse<PagedResult<USerProgressDTo>>.Fail("There are no users with this plan id", 404);
            var query = model.AsQueryable();
           var Model = validationquery(query,request.PageNumber,request.PageSize,request.SearchWeight,request.Date);


            return RequestResponse<PagedResult<USerProgressDTo>>.Success(Model, "Users retrieved successfully", 200);
        }
        private PagedResult<USerProgressDTo> validationquery(IQueryable<UserProgress> Data, int PageNumber, int PageSize, decimal? SearchWeight, DateOnly? Date)
        {
            int pageSize = PageSize <= 0 ? 10 : PageSize;
            int pageNumber = PageNumber <= 0 ? 1 : PageNumber;
            decimal? searchWeight = (SearchWeight > 0 && SearchWeight < 200) ? null : SearchWeight;
            DateOnly? date = Date < DateOnly.FromDateTime(DateTime.Now) ? Date : null;
            if (date.HasValue)
                Data = Data.Where(e => e.MeasurementDate == Date);
            if (searchWeight.HasValue)
                Data = Data.Where(e => e.CurrentWeight == searchWeight);
            int totalCount = Data.Count();
            var userprogresAll = Data.
                 Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize).Select(e => new USerProgressDTo
                 {
                     CurrentWeight = e.CurrentWeight,
                     MeasurementDate = e.MeasurementDate,
                 }).ToList();     
            var model2 = new PagedResult<USerProgressDTo>
            {
                  Item = userprogresAll,
                  PageNumber = pageNumber,
                  PageSize  = pageSize,
                  TotalCount = totalCount
            };
            return model2;
        }
    }

}
