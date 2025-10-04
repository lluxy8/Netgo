using MediatR;
using Netgo.Application.Common;
using Netgo.Application.DTOs.Product;

namespace Netgo.Application.Features.Products.Requests.Query
{
    public class GetProductsQuery : IRequest<Result<List<ListProductDTO>>>
    {
        public required GetProductsDTO Filter { get; set; }
    }
}
