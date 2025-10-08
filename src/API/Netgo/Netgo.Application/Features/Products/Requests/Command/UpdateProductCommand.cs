using MediatR;
using Netgo.Application.Common;
using Netgo.Application.DTOs.Product;

namespace Netgo.Application.Features.Products.Requests.Command
{
    public class UpdateProductCommand : IRequest<Result>
    {
        public required string UserId { get; set; }
        public UpdateProductDTO ProductDto { get; set; }
    }
}
