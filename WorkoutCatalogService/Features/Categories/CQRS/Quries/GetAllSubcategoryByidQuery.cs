using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache memoryCache;
        private const string CacheKey = "AllSubCategories";

        public GetAllSubcategoryByidQueryHandler(IGenericRepository<SubCategory> genericRepository,IMemoryCache memoryCache)
        {
            this.genericRepository = genericRepository;
            this.memoryCache = memoryCache;
        }
        public async Task<RequestResponse<IEnumerable<SubCategoryDTo>>> Handle(GetAllSubcategoryByidQuery request, CancellationToken cancellationToken)
        {
            if (memoryCache.TryGetValue(CacheKey, out IEnumerable<SubCategoryDTo> subcategory))
            {
                return RequestResponse<IEnumerable<SubCategoryDTo>>.Success(subcategory);
            }
           
            var subcategories = await genericRepository.GetAll().Select(sc => new SubCategoryDTo
            {
                Name = sc.Name,
                Description = sc.Description
            }).ToListAsync();

           memoryCache.Set(CacheKey, subcategories);


            if (subcategories.Count == 0)
            {
                return RequestResponse<IEnumerable<SubCategoryDTo>>.Fail("No subcategories found for the provided IDs.", 404);
            }
            return RequestResponse<IEnumerable<SubCategoryDTo>>.Success(subcategories);
        }
    }


}
