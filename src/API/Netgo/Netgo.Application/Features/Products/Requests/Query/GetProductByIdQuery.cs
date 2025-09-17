using MediatR;
using Netgo.Application.Common;
using Netgo.Application.DTOs.Product;

namespace Netgo.Application.Features.Products.Requests.Query
{
    public class GetProductByIdQuery : IRequest<Result<ProductDTO>>
    {
        public Guid Id { get; set; }
    }
}
