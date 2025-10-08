using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Netgo.Application.Common;
using Netgo.Application.DTOs.Product;
using Netgo.Application.Features.Products.Handlers.Query;
using Netgo.Application.Features.Products.Requests.Command;
using Netgo.Application.Features.Products.Requests.Query;
using System.Security.Claims;

namespace Netgo.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    [EnableRateLimiting("sliding")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] GetProductsFilterDTO filter)
        {
            var request = new GetProductsQuery { Filter = filter};
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }

        [HttpGet("myproducts")]
        public async Task<IActionResult> GetMyProducts(GetProductsFilterDTO filter)
        {
            var id = User.FindFirstValue(CustomClaimTypes.Uid);
            if (id is null)
                return Unauthorized();

            var request = new GetUserProductsQuery { Id = Guid.Parse(id), Filter = filter };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var request = new GetProductByIdQuery { Id = id };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDTO product)
        {
            var id = User.FindFirstValue(CustomClaimTypes.Uid);
            if (id is null)
                return Forbid();

            var request = new CreateProductCommand { UserId = id, ProductDto = product };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductDTO product)
        {
            var id = User.FindFirstValue(CustomClaimTypes.Uid);
            if (id is null)
                return Forbid();

            var request = new UpdateProductCommand { UserId = id, ProductDto = product };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetProductsByUserId(Guid userId, GetProductsFilterDTO filter)
        {
            var request = new GetUserProductsQuery { Id = userId, Filter = filter };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }


    }
}
