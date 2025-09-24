using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Netgo.Application.Common;
using Netgo.Application.DTOs.Category;
using Netgo.Application.Features.Category.Requests.Command;
using Netgo.Application.Features.Category.Requests.Query;

namespace Netgo.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [EnableRateLimiting("sliding")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var request = new GetCategoryByIdQuery { Id = id };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var request = new GetCategoriesQuery();
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }


        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDTO dto)
        {
            var command = new CreateCategoryCommand { CategoryDto = dto };
            var result = await _mediator.Send(command);
            return new ResultActionResult(result);
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Update([FromBody] CategoryUpdateDTO dto)
        {
            var request = new UpdateCategoryCommand { CategoryDTO = dto };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = new DeleteCategoryCommand { Id = id };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }
    }
}
