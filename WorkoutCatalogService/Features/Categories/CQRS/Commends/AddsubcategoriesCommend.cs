using Azure.Core;
using MediatR;
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
    public record AddsubcategoriesCommend(Guid CategoryId, IEnumerable<SubCategoryDTo> SubCategoryDTos):IRequest<RequestResponse<IEnumerable<SubCategoryDTo>>>;

    public class AddsubcategoriesCommendHandler : IRequestHandler<AddsubcategoriesCommend, RequestResponse<IEnumerable<SubCategoryDTo>>>
    {
        private readonly IGenericRepository<SubCategory> genericRepository;

        public AddsubcategoriesCommendHandler(IGenericRepository<SubCategory> genericRepository)
        {
            this.genericRepository = genericRepository;
        }
        public async Task<RequestResponse<IEnumerable<SubCategoryDTo>>> Handle(AddsubcategoriesCommend request, CancellationToken cancellationToken)
        {

         

            var mappedSubcategories = request.SubCategoryDTos.Select(dto => new SubCategory
            {
                Description = dto.Description,
                Name = dto.Name,
                CategoryId = request.CategoryId,
                
            }).ToList();
            await genericRepository.AddRangeAsync(mappedSubcategories);
            await genericRepository.SaveChanges();

            return RequestResponse<IEnumerable<SubCategoryDTo>>.Success(request.SubCategoryDTos, "subcategories added successfully", 200);
        }

       
    }




}
