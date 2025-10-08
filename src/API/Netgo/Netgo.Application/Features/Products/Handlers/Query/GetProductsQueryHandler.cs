using AutoMapper;
using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Persistence;
using Netgo.Application.DTOs.Product;
using Netgo.Application.Exceptions;
using Netgo.Application.Features.Products.Requests.Query;
using Netgo.Application.Filters;
using Netgo.Application.Models;
using Netgo.Domain;
using System.Linq.Expressions;

namespace Netgo.Application.Features.Products.Handlers.Query
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<PagedResult<ListProductDTO>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<ListProductDTO>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var filter = ProductFilter.Filter(request.Filter);

            var products = await _productRepository.GetAllFilteredPaged(
                new PagedFilter<Product>
                {
                    Filter = filter,
                    PageSize = request.Filter.PageSize,
                    Page = request.Filter.Page
                });

            if (products.TotalCount == 0)
                throw new NotFoundException("Products", "with filter");

            var productsMapped = _mapper.Map<List<ListProductDTO>>(products.Items);
            
            foreach(var i in productsMapped)
            {
                i.Image = products.Items.Where(x => 
                    x.Id == i.Id)
                    .FirstOrDefault()!
                    .Images
                    .FirstOrDefault() 
                    ?? "";
            }

            return Result<PagedResult<ListProductDTO>>.Success(new PagedResult<ListProductDTO>
            {
                Items = productsMapped,
                PageSize = products.PageSize,
                Page = products.Page,
                RemainingCount = products.RemainingCount,
                TotalCount = products.TotalCount
            });
        }
    }
}
