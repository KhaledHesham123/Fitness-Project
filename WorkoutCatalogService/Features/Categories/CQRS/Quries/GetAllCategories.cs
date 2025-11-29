using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WorkoutCatalogService.Features.Categories.DTOs;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.Categories.CQRS.Quries
{
    public record GetAllCategories:IRequest<RequestResponse<IEnumerable<CategoriesDTO>>>;

    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategories, RequestResponse<IEnumerable<CategoriesDTO>>>
    {
        private readonly IGenericRepository<category> _categoryRepo;
        private readonly IMemoryCache memoryCache;
        private const string CacheKey = "AllCategories"; 

        public GetAllCategoriesHandler(IGenericRepository<category> CategoryRepo,IMemoryCache memoryCache)
        {
            _categoryRepo = CategoryRepo;
            this.memoryCache = memoryCache;
        }
        public async Task<RequestResponse<IEnumerable<CategoriesDTO>>> Handle(GetAllCategories request, CancellationToken cancellationToken)
        {

            if (memoryCache.TryGetValue(CacheKey, out IEnumerable<CategoriesDTO> cachedCategories))
            {
                return RequestResponse<IEnumerable<CategoriesDTO>>.Success(cachedCategories, "Fetched from cache");
            }
            var mappedCategories = await _categoryRepo.GetAll()
                           .Select(x => new CategoriesDTO
                           {
                               Name = x.Name,
                               Description = x.Description,

                               SubCategories = x.SubCategories
                                   .Select(sc => new SubCategoryDTo
                                   {
                                       Name = sc.Name,
                                       Description = sc.Description
                                   })
                                   .ToList()
                           })
                           .ToListAsync(); 

            if (mappedCategories == null)
            {
              return RequestResponse<IEnumerable<CategoriesDTO>>.Fail("Category is empty", 400);
            }

            memoryCache.Set(CacheKey, mappedCategories, TimeSpan.FromMinutes(120));



            return RequestResponse<IEnumerable<CategoriesDTO>>.Success(mappedCategories);


        }
    }
}
