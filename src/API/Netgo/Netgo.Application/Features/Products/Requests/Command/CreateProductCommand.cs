using MediatR;
using Netgo.Application.Common;
using Netgo.Application.DTOs.Product;

namespace Netgo.Application.Features.Products.Requests.Command
{
    public class CreateProductCommand : IRequest<Result<Guid>> 
    {
        public required string UserId { get; set; }
        public required CreateProductDTO ProductDto { get; set; }
    }
}
