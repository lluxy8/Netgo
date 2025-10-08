using MediatR;
using Netgo.Application.Common;
using Netgo.Application.DTOs.Product;

namespace Netgo.Application.Features.Products.Requests.Query
{
    public class GetUserProductsQuery : IRequest<Result<List<ListProductDTO>>>
    {
        public required GetProductsFilterDTO Filter { get; set; }
        public required Guid Id { get; set; }
    }
}
