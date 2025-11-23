using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutCatalogService.Features.Categories.DTOs;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.Categories.CQRS.Quries
{
    public record GetAllSubcategoryByidQuery(IEnumerable<Guid> Subcategoryid):IRequest<RequestResponse<IEnumerable<SubCategoryDTo>>>;
    
    public class GetAllSubcategoryByidQueryHandler: IRequestHandler<GetAllSubcategoryByidQuery, RequestResponse<IEnumerable<SubCategoryDTo>>>
    {
        private readonly IGenericRepository<SubCategory> genericRepository;
        public GetAllSubcategoryByidQueryHandler(IGenericRepository<SubCategory> genericRepository)
        {
            this.genericRepository = genericRepository;
        }
        public async Task<RequestResponse<IEnumerable<SubCategoryDTo>>> Handle(GetAllSubcategoryByidQuery request, CancellationToken cancellationToken)
        {
            var subcategories = await genericRepository.GetAll().Select(sc => new SubCategoryDTo
            {
                Name = sc.Name,
                Description = sc.Description
            }).ToListAsync();

           


            if (subcategories.Count == 0)
            {
                return RequestResponse<IEnumerable<SubCategoryDTo>>.Fail("No subcategories found for the provided IDs.", 404);
            }
            return RequestResponse<IEnumerable<SubCategoryDTo>>.Success(subcategories);
        }
    }


}
