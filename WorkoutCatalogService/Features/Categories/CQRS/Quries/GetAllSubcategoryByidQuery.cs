using MediatR;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.Categories.CQRS.Quries
{
    public record GetAllSubcategoryByidQuery(IEnumerable<Guid> Subcategoryid):IRequest<RequestResponse<IEnumerable<SubCategory>>>;
    
    public class GetAllSubcategoryByidQueryHandler: IRequestHandler<GetAllSubcategoryByidQuery, RequestResponse<IEnumerable<SubCategory>>>
    {
        private readonly IGenericRepository<SubCategory> genericRepository;
        public GetAllSubcategoryByidQueryHandler(IGenericRepository<SubCategory> genericRepository)
        {
            this.genericRepository = genericRepository;
        }
        public async Task<RequestResponse<IEnumerable<SubCategory>>> Handle(GetAllSubcategoryByidQuery request, CancellationToken cancellationToken)
        {
            var subcategories = new List<SubCategory>();
            foreach (var id in request.Subcategoryid)
            {
                var result = await genericRepository.FindAsync(x=>x.Id==id);
                var subcategory = result.FirstOrDefault();

                if (subcategory != null)
                {
                    subcategories.Add(subcategory);
                }
            }
            if (subcategories.Count == 0)
            {
                return RequestResponse<IEnumerable<SubCategory>>.Fail("No subcategories found for the provided IDs.", 404);
            }
            return RequestResponse<IEnumerable<SubCategory>>.Success(subcategories);
        }
    }


}
