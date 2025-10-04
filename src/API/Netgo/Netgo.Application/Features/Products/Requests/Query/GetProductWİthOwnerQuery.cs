using MediatR;
using Netgo.Application.Common;
using Netgo.Application.DTOs.Product;

namespace Netgo.Application.Features.Products.Requests.Query
{
    public class GetProductWithOwnerQuery : IRequest<Result<ProductWithOwnerDTO>>
    {
        public Guid ProductId { get; set; }
    }
}
