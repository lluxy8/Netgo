using MediatR;
using Netgo.Application.Common;
using Netgo.Application.DTOs.Product;

namespace Netgo.Application.Features.Products.Requests.Command
{
    public class UpdateProductCommand : IRequest<Result>
    {
        public UpdateProductDTO ProductDto { get; set; }
    }
}
