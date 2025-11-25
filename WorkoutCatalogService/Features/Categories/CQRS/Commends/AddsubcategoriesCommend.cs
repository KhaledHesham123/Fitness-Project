using Azure.Core;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using System.ComponentModel.DataAnnotations;
using WorkoutCatalogService.Features.Categories.DTOs;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.Response;
using WorkoutCatalogService.Shared.Srvieces;

namespace WorkoutCatalogService.Features.Categories.CQRS.Commends
{
    public record AddsubcategoriesCommend(Guid CategoryId, IEnumerable<SubCategoryDTo> SubCategoryDTos):IRequest<RequestResponse<bool>>;

    public class AddsubcategoriesCommendHandler : IRequestHandler<AddsubcategoriesCommend, RequestResponse<bool>>
    {
        private readonly IGenericRepository<SubCategory> genericRepository;
        private readonly IMemoryCache memoryCache;

        public AddsubcategoriesCommendHandler(IGenericRepository<SubCategory> genericRepository,IMemoryCache memoryCache)
        {
            this.genericRepository = genericRepository;
            this.memoryCache = memoryCache;
        }
        public async Task<RequestResponse<bool>> Handle(AddsubcategoriesCommend request, CancellationToken cancellationToken)
        {

         

            var Subcategories = request.SubCategoryDTos.Select(dto => new SubCategory
            {
                Description = dto.Description,
                Name = dto.Name,
                CategoryId = request.CategoryId,
                
            }).ToList();


            await genericRepository.AddRangeAsync(Subcategories);
            await genericRepository.SaveChanges();

            foreach (var subcategory in Subcategories) 
            {
                memoryCache.Set(subcategory.Id, new SubCategoryDTo
                {
                    Name = subcategory.Name,
                    Description = subcategory.Description,
                    CategoryId = subcategory.CategoryId
                }, TimeSpan.FromHours(2));
            }

                var mappedsubcategories = Subcategories.Select(sc => new SubCategoryDTo
                {
                Name = sc.Name,
                Description = sc.Description,
                CategoryId = sc.CategoryId
                });

            return RequestResponse<bool>.Success(true, "subcategories added successfully", 200);
        }

       
    }




}
