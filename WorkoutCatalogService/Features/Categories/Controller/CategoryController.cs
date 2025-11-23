using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WorkoutCatalogService.Features.Categories.CQRS.Orchestratots;
using WorkoutCatalogService.Features.Categories.CQRS.Quries;
using WorkoutCatalogService.Features.Categories.DTOs;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.Categories.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator mediator;

        public CategoryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("GetAllCategories")] // GET: api/Category/GetAllCategories
        public async Task<ActionResult<EndpointResponse<IEnumerable<CategoriesDTO>>>> GetAllCategories()
        {
            var categoriesResult = await mediator.Send(new GetAllCategories());

            var response = new EndpointResponse<IEnumerable<CategoriesDTO>>
            {
                IsSuccess = categoriesResult.IsSuccess,
                Message = categoriesResult.Message,
                Data = categoriesResult.Data
            };

            if (!categoriesResult.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("addCategory")] // PUT: api/Category/addCategory
        public async Task<ActionResult<EndpointResponse<bool>>> AddCategory([FromBody] CategoryToaddDTO category)
        {
            var result = await mediator.Send(new AddCategoryOrchestrator(category.Name,category.Description,category.SubCategories));

            var response = new EndpointResponse<bool>
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Data = result.Data
            };

            if (!result.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
