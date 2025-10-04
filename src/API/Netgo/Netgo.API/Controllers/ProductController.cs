using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netgo.Application.DTOs.Product;
using Netgo.Application.Features.Products.Requests.Command;
using Netgo.Application.Features.Products.Requests.Query;

namespace Netgo.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var request = new GetProductsQuery();
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var request = new GetProductByIdQuery { Id = id };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }

        [HttpPost]
<<<<<<< Updated upstream
        //[Authorize]
        public async Task<IActionResult> CreateProduct(CreateProductDTO product)
=======
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO product)
>>>>>>> Stashed changes
        {
            var request = new CreateProductCommand { ProductDto = product };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }

        [HttpPut]
<<<<<<< Updated upstream
        //[Authorize]
        public async Task<IActionResult> UpdateProduct(UpdateProductDTO product)
=======
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDTO product)
>>>>>>> Stashed changes
        {
            var request = new UpdateProductCommand { ProductDto = product };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }

        [HttpGet("user/{id:guid}")]
        public async Task<IActionResult> GetProductsByUserId(Guid id)
        {
            var request = new GetUserProductsQuery { Id = id };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }

        [HttpGet("{id:guid}/withowner")]
        public async Task<IActionResult> GetProductWithOwner(Guid id)
        {
            var request = new GetProductWithOwnerQuery { ProductId = id };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }
    }
}
