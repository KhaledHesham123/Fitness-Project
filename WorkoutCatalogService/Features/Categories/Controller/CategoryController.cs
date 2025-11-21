using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WorkoutCatalogService.Features.Categories.CQRS.Quries;
using WorkoutCatalogService.Shared.Entites;

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
        public async Task<IActionResult> GetAllCategories()
        {
            var Categories = await mediator.Send(new GetAllCategories());
            return Ok(Categories);
        }
    }
}
